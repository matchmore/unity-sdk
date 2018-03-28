using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.Collections.Generic;
using MatchmorePersistence;
using System.Collections;
using System.Collections.ObjectModel;
using MatchmoreLocation;
using MatchmoreUtils;

public partial class Matchmore
{
    private static Matchmore _instance;
    public static readonly string API_VERSION = "v5";
    public static readonly string PRODUCTION = "api.matchmore.io";
    private ApiClient _client;
    private DeviceApi _deviceApi;
    private StateManager _state;
    private GameObject _obj;
    private CoroutineWrapper _coroutine;
    private string _environment;
    private string _apiKey;
    private bool _secured;
    private int? _servicePort;
    private Dictionary<string, IMatchMonitor> _monitors = new Dictionary<string, IMatchMonitor>();
    private List<EventHandler<MatchReceivedEventArgs>> _eventHandlers = new List<EventHandler<MatchReceivedEventArgs>>();
    private readonly Config _config;
    private ILocationService _locationService;

    public event EventHandler<MatchReceivedEventArgs> MatchReceived
    {
        add
        {
            foreach (var monitor in _monitors)
            {
                _eventHandlers.Add(value);
                monitor.Value.MatchReceived += value;
            }
        }
        remove
        {
            foreach (var monitor in _monitors)
            {
                _eventHandlers.Remove(value);
                monitor.Value.MatchReceived -= value;
            }
        }
    }

    /// <summary>
    /// Match channel via which the matches will be delivered
    /// </summary>
    [Flags]
    public enum MatchChannel
    {
        polling = 0,
        websocket = 1,
        threadedPolling = 2
    }

    /// <summary>
    /// Gets the game object to which some feature ar attached
    /// </summary>
    /// <value>The object.</value>
    public GameObject Obj { get { return _obj; } }

    public Dictionary<string, IMatchMonitor> Monitors
    {
        get
        {
            return _monitors;
        }
    }

    public Device MainDevice
    {
        get
        {
            return _state.Device;
        }
        private set
        {
            _state.Device = value;
            _state.Save();
        }
    }

    /// <summary>
    /// Mock location for development purposes which will be used instead of the device location
    /// </summary>
    /// <value>The mock location.</value>
    public Location MockLocation { get; set; }

    /// <summary>
    /// Gets the API URL.
    /// </summary>
    /// <value>The API URL.</value>
    public string ApiUrl
    {
        get
        {
            if (_environment != null)
            {
                var protocol = _secured ? "https" : "http";
                var port = _servicePort == null ? "" : ":" + _servicePort;
                return String.Format("{2}://{0}{3}/{1}", _environment, API_VERSION, protocol, port);
            }
            else
            {
                var protocol = _secured ? "https" : "http";
                return String.Format("{0}://{1}/{2}", protocol, PRODUCTION, API_VERSION);
            }
        }
    }

    public static void Configure(string apiKey)
    {
        Configure(Config.WithApiKey(apiKey));
    }

    public static void Configure(Config config)
    {
        if (_instance != null)
        {
            throw new InvalidOperationException("Matchmore static instance already configured");
        }
        _instance = new Matchmore(config);
    }

    public static void Reset()
    {
        if (_instance != null)
        {
            _instance.CleanUp();
            _instance = null;
        }
    }

    //
    public static Matchmore Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("Matchmore not initialized!!!");
            }
            return _instance;
        }
    }

    public Matchmore(Config config)
    {
        _config = config;

        MatchmoreLogger.Enabled = config.LoggingEnabled;

        if (string.IsNullOrEmpty(config.ApiKey))
        {
            throw new ArgumentException("Api key null or empty");
        }

        _apiKey = config.ApiKey;
        _servicePort = config.ServicePort;
        _environment = config.Environment ?? PRODUCTION;
        _secured = config.UseSecuredCommunication;
        InitGameObjects();

        _state = new StateManager(_environment, config.PersistenceFile);

        if (MainDevice == null)
        {
            CreateDevice(new MobileDevice(), makeMain: true);
        }

        _coroutine.SetupContinuousRoutine("persistence", _state.PruneDead);
    }

    /// <summary>
    /// Starts the location service with specified type
    /// Coroutined, uses a dedicated coroutine
    /// Threaded, creates an ongoing thread which polls the Unity location service and reports it to matchmore
    /// </summary>
    /// <param name="type">Type,</param>
    public void StartLocationService(LocationServiceType type)
    {
        if (_locationService != null)
        {
            _locationService.Stop();
        }
        Action<Location> update = loc => UpdateLocation(loc);
        switch (type)
        {
            case LocationServiceType.coroutine:
                _locationService = new CoroutinedLocationService(_coroutine, update);
                break;
            case LocationServiceType.threaded:
                _locationService = new ThreadedLocationSercixe(_coroutine, update);
                break;
        }
        _locationService.MockLocation = MockLocation;
        _locationService.Start();
    }

    public void WipeData()
    {
        _state.WipeData();
    }

    private void InitGameObjects()
    {
        _client = new ApiClient(ApiUrl);
        _client.AddDefaultHeader("api-key", _apiKey);
        _deviceApi = new DeviceApi(_client);
        _obj = new GameObject("MatchMoreObject");
        if (_config.LoggingEnabled)
        {
            MatchmoreLogger.Context = _obj;
        }
        _coroutine = _obj.AddComponent<CoroutineWrapper>();
    }

    /// <summary>
    /// Creates the device.
    /// </summary>
    /// <returns>The device.</returns>
    /// <param name="device">Device.</param>
    /// <param name="makeMain">If set to <c>true</c> make main.</param>
    public Device CreateDevice(Device device, bool makeMain = false)
    {
        if (_state == null)
        {
            throw new InvalidOperationException("Persistence wasn't setup!!!");
        }

        Device createdDevice = null;

        if (!string.IsNullOrEmpty(device.Id))
        {
            UnityEngine.MonoBehaviour.print("Device ID will be ignored!!!");
        }

        if (device is PinDevice)
        {
            var pinDevice = device as PinDevice;
            pinDevice.DeviceType = Alps.Model.DeviceType.Pin;
            if (pinDevice.Location == null)
            {
                throw new ArgumentException("Location required for Pin Device");
            }

            createdDevice = pinDevice;
        }

        if (device is MobileDevice)
        {
            var mobileDevice = device as MobileDevice;
            mobileDevice.DeviceType = Alps.Model.DeviceType.Mobile;

            mobileDevice.Name = string.IsNullOrEmpty(mobileDevice.Name) ? SystemInfo.deviceModel : mobileDevice.Name;
            mobileDevice.Platform = string.IsNullOrEmpty(mobileDevice.Platform) ? Application.platform.ToString() : mobileDevice.Platform;
            mobileDevice.DeviceToken = string.IsNullOrEmpty(mobileDevice.DeviceToken) ? "" : mobileDevice.DeviceToken;

            createdDevice = mobileDevice;
        }

        if (device is IBeaconDevice)
        {
            var ibeaconDevice = device as IBeaconDevice;
            ibeaconDevice.DeviceType = Alps.Model.DeviceType.IBeacon;
            if (ibeaconDevice.Major == null)
            {
                throw new ArgumentException("Major required for Ibeacon Device");
            }

            if (ibeaconDevice.Minor == null)
            {
                throw new ArgumentException("Minor required for Ibeacon Device");
            }

            if (string.IsNullOrEmpty(ibeaconDevice.Name))
            {
                throw new ArgumentException("Name required for Ibeacon Device");
            }

            createdDevice = ibeaconDevice;
        }

        var deviceInBackend = _deviceApi.CreateDevice(createdDevice);
        //only mobile can be considered as a main device
        if (makeMain && createdDevice is MobileDevice)
        {
            MainDevice = deviceInBackend;
        }
        return deviceInBackend;
    }

    /// <summary>
    /// Creates the pin device.
    /// </summary>
    /// <returns>The pin device.</returns>
    /// <param name="pinDevice">Pin device.</param>
    public PinDevice CreatePinDevice(PinDevice pinDevice, bool ignorePersistence = false)
    {
        pinDevice.DeviceType = Alps.Model.DeviceType.Pin;
        if (pinDevice.Location == null)
        {
            throw new ArgumentException("Location required for Pin Device");
        }

        var createdDevice = _deviceApi.CreateDevice(pinDevice);

        //The generated swagger api returns a generic device partially losing the information about the pin.
        //We rewrite the data to fit the pin device contract.
        var createdPin = new PinDevice
        {
            Id = createdDevice.Id,
            CreatedAt = createdDevice.CreatedAt,
            DeviceType = createdDevice.DeviceType,
            Location = pinDevice.Location,
            Group = createdDevice.Group,
            Name = createdDevice.Name,
            UpdatedAt = createdDevice.UpdatedAt
        };
        if (!ignorePersistence)
            _state.AddPinDevice(createdPin);

        return createdPin;
    }

    /// <summary>
    /// Creates the pin device and start listening. This is useful when the device also manages pins
    /// </summary>
    /// <returns>The pin device and start listening.</returns>
    /// <param name="pinDevice">Pin device.</param>
    /// <param name="channel">Channel.</param>
    public MTuple<PinDevice, IMatchMonitor> CreatePinDeviceAndStartListening(PinDevice pinDevice, MatchChannel channel)
    {
        var createdDevice = CreatePinDevice(pinDevice);
        var monitor = SubscribeMatches(channel, createdDevice);

        return MTuple.New(createdDevice, monitor);
    }

    /// <summary>
    /// Creates the subscription
    /// </summary>
    /// <returns>The subscription.</returns>
    /// <param name="sub">Sub.</param>
    /// <param name="device">Device, if null will default to main device</param>
    public Subscription CreateSubscription(Subscription sub, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return CreateSubscription(sub, usedDevice.Id);
    }

    /// <summary>
    /// Creates the subscription.
    /// </summary>
    /// <returns>The subscription.</returns>
    /// <param name="sub">Sub.</param>
    /// <param name="deviceId">Device identifier.</param>
    /// <param name="ignorePersistence">If set to <c>true</c> ignore persistence.</param>
    public Subscription CreateSubscription(Subscription sub, string deviceId, bool ignorePersistence = false)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        var _sub = _deviceApi.CreateSubscription(deviceId, sub);
        if (!ignorePersistence)
            _state.AddSubscription(_sub);
        return _sub;
    }

    /// <summary>
    /// Creates the publication.
    /// </summary>
    /// <returns>The publication.</returns>
    /// <param name="pub">Pub.</param>
    /// <param name="device">Device, if null will default to main device</param>
    public Publication CreatePublication(Publication pub, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return CreatePublication(pub, usedDevice.Id);
    }

    /// <summary>
    /// Creates the publication.
    /// </summary>
    /// <returns>The publication.</returns>
    /// <param name="pub">Pub.</param>
    /// <param name="deviceId">Device identifier.</param>
    /// /// <param name="ignorePersistence">If set to <c>true</c> ignore persistence.</param>
    public Publication CreatePublication(Publication pub, string deviceId, bool ignorePersistence = false)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        var _pub = _deviceApi.CreatePublication(deviceId, pub);
        if (!ignorePersistence)
            _state.AddPublication(_pub);
        return _pub;
    }

    /// <summary>
    /// Updates the location.
    /// </summary>
    /// <returns>The location.</returns>
    /// <param name="location">Location.</param>
    /// <param name="device">Device, if null will default to main device</param>
    public Location UpdateLocation(Location location, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return UpdateLocation(location, usedDevice.Id);
    }

    /// <summary>
    /// Updates the location.
    /// </summary>
    /// <returns>The location.</returns>
    /// <param name="location">Location.</param>
    /// <param name="deviceId">Device identifier.</param>
    public Location UpdateLocation(Location location, string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        if (location.Altitude == null)
            location.Altitude = 0;

        return _deviceApi.CreateLocation(deviceId, location);
    }


    /// <summary>
    /// Gets the matches.
    /// </summary>
    /// <returns>The matches.</returns>
    /// <param name="device">Device, if null will default to main device</param>
    public List<Match> GetMatches(Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return GetMatches(usedDevice.Id);
    }

    /// <summary>
    /// Gets the matches.
    /// </summary>
    /// <returns>The matches.</returns>
    /// <param name="deviceId">Device identifier.</param>
    public List<Match> GetMatches(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        return _deviceApi.GetMatches(deviceId);
    }

    /// <summary>
    /// Subscribes the matches.
    /// </summary>
    /// <returns>The matches.</returns>
    /// <param name="channel">Channel.</param>
    /// <param name="deviceId">Device identifier.</param>
    public IMatchMonitor SubscribeMatches(MatchChannel channel, string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        return SubscribeMatches(channel, FindDevice(deviceId));
    }

    /// <summary>
    /// Subscribes the matches.
    /// </summary>
    /// <returns>The matches.</returns>
    /// <param name="channel">Channel.</param>
    /// <param name="device">Device, if null will default to main device</param>
    public IMatchMonitor SubscribeMatches(MatchChannel channel, Device device = null)
    {
        var deviceToSubscribe = device == null ? _state.Device : device;
        IMatchMonitor monitor = null;
        switch (channel)
        {
            case MatchChannel.polling:
                monitor = CreatePollingMonitor(deviceToSubscribe);
                break;
            case MatchChannel.websocket:
                monitor = CreateWebsocketMonitor(deviceToSubscribe);
                break;
            case MatchChannel.threadedPolling:
                monitor = CreateThreadedPollingMonitor(deviceToSubscribe);
                break;
            default:
                break;
        }

        if (monitor == null)
        {
            throw new ArgumentException(String.Format("{0} is an unrecognized channel", channel));
        }

        if (_monitors.ContainsKey(deviceToSubscribe.Id))
        {
            _monitors[deviceToSubscribe.Id].Stop();
            _monitors.Remove(deviceToSubscribe.Id);
        }

        foreach (var handler in _eventHandlers)
        {
            monitor.MatchReceived += handler;
        }

        _monitors.Add(deviceToSubscribe.Id, monitor);

        return monitor;
    }

    private Device FindDevice(string deviceId)
    {
        if (_state.Device.Id == deviceId)
            return _state.Device;
        else
            return _state.Pins.Find(pin => pin.Id == deviceId);
    }

    /// <summary>
    /// Gets the active subscriptions.
    /// </summary>
    /// <value>The active subscriptions.</value>
    public IEnumerable<Subscription> ActiveSubscriptions
    {
        get
        {
            return _state.ActiveSubscriptions.AsReadOnly();
        }
    }

    /// <summary>
    /// Gets the active publications.
    /// </summary>
    /// <value>The active publications.</value>
    public IEnumerable<Publication> ActivePublications
    {
        get
        {
            return _state.ActivePublications.AsReadOnly();
        }
    }

    /// <summary>
    /// Cleans up. Stops all monitors, destroys the game object
    /// </summary>
    public void CleanUp()
    {
        StopEverything();
        if (_coroutine != null)
        {
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(_coroutine);
            else
                UnityEngine.Object.Destroy(_coroutine);
        }
        if (_obj != null)
        {
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(_obj);
            else
                UnityEngine.Object.Destroy(_obj);
        }
    }

    public void StopEverything()
    {
        var keys = new List<string>(_monitors.Keys);
        foreach (var key in keys)
        {
            IMatchMonitor monitor = null;
            if (_monitors.TryGetValue(key, out monitor))
                monitor.Stop();
        }

        _monitors.Clear();
        if (_locationService != null)
            _locationService.Stop();

    }

    private PollingMatchMonitor CreatePollingMonitor(Device device)
    {
        return new PollingMatchMonitor(device, _deviceApi, _coroutine, id => _monitors.Remove(id));
    }

    private ThreadedPollingMatchMonitor CreateThreadedPollingMonitor(Device device)
    {
        return new ThreadedPollingMatchMonitor(device, _deviceApi, _coroutine, id => _monitors.Remove(id));
    }

    private WebsocketMatchMonitor CreateWebsocketMonitor(Device device)
    {
        return new WebsocketMatchMonitor(device, _config, _deviceApi, _coroutine, id => _monitors.Remove(id));
    }
}
