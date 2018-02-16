using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using System.Text;
using Newtonsoft.Json;
using MatchmorePersistence;
using System.Collections;

public class Matchmore
{
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
    private string _worldId;
    private int? _servicePort;
    private int? _pusherPort;
    private Dictionary<string, MatchMonitor> _monitors = new Dictionary<string, MatchMonitor>();

    public Dictionary<string, MatchMonitor> Monitors
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

    /// <summary>
    /// Configure the specified apiKey, environment, useSecuredCommunication, startWebsocketImmediately and worldId.
    /// </summary>
    /// <returns>The configure.</returns>
    /// <param name="apiKey">API key received from the Matchmore portal</param>
    /// <param name="environment">Environment, by default it will be production</param>
    /// <param name="useSecuredCommunication">If set to <c>true</c> use secured communication.</param>
    /// <param name="startWebsocketImmediately">If set to <c>true</c> start websocket immediately.</param>
    /// <param name="worldId">World identifier.</param>
    public static void Configure(string apiKey, string environment = null, bool useSecuredCommunication = true, bool startWebsocketImmediately = false, string worldId = null)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Matchmore static instance already configured");
        }
        Instance = new Matchmore(apiKey, environment, useSecuredCommunication, startWebsocketImmediately, worldId);
    }

    public static void Reset()
    {
        if (_instance != null)
        {
            _instance.CleanUp();
            _instance = null;

        }
    }

    private static Matchmore _instance;

    public static Matchmore Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public Matchmore(string apiKey, string environment = null, bool useSecuredCommunication = true, bool starWebsocketImmediately = false, string worldId = null, int? servicePort = null, int? pusherPort = null)
    {
        _servicePort = servicePort;
        _pusherPort = pusherPort;

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("Api key null or empty");
        }

        if (string.IsNullOrEmpty(worldId))
        {
            var deserializedApiKey = Utils.ExtractWorldId(apiKey);
            _worldId = deserializedApiKey;
        }
        else
        {
            _worldId = worldId;
        }

        _environment = environment;
        _apiKey = apiKey;
        _secured = useSecuredCommunication;

        Init();

        _state = new StateManager();
        if (MainDevice == null)
        {
            CreateDevice(new MobileDevice(), makeMain: true);
        }

        _coroutine.SetupContinuousRoutine("persistence", _state.CheckDuration);
        _coroutine.RunOnce("location_service", StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
# if !UNITY_IOS
            //https://docs.unity3d.com/ScriptReference/LocationService-isEnabledByUser.html
            //if it is IOS we do not break here
            yield break;
#endif
        }


        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            //print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //print("Unable to determine device location");
            yield break;
        }

        _coroutine.SetupContinuousRoutine("location_service", UpdateLocation);
    }

    private void UpdateLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var location = Input.location.lastData;

            UpdateLocation(new Location
            {
                Latitude = location.latitude,
                Longitude = location.longitude,
                Altitude = location.altitude
            });
        }
    }

    private void Init()
    {
        _client = new ApiClient(ApiUrl);
        _client.AddDefaultHeader("api-key", _apiKey);
        _deviceApi = new DeviceApi(_client);
        _obj = new GameObject("MatchMoreObject");
        _coroutine = _obj.AddComponent<CoroutineWrapper>();
    }

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

    public PinDevice CreatePinDevice(PinDevice pinDevice)
    {
        pinDevice.DeviceType = Alps.Model.DeviceType.Pin;
        if (pinDevice.Location == null)
        {
            throw new ArgumentException("Location required for Pin Device");
        }

        var createdDevice = (PinDevice)_deviceApi.CreateDevice(pinDevice);
        _state.AddPinDevice(createdDevice);
  
        return createdDevice;
    }

    public Tuple<PinDevice, MatchMonitor> CreatePinDeviceAndStartListening(PinDevice pinDevice, Action<List<Match>> onMatchCallback)
    {
        var createdDevice = CreatePinDevice(pinDevice);
        var monitor = SubscribeMatchesWithWS(onMatchCallback, createdDevice);

        return Tuple.New(createdDevice, monitor);
    }

    public Subscription CreateSubscription(Subscription sub, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return CreateSubscription(sub, usedDevice.Id);
    }

    public Subscription CreateSubscription(Subscription sub, string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        var _sub = _deviceApi.CreateSubscription(deviceId, sub);
        _state.AddSubscription(_sub);
        return _sub;
    }

    public Publication CreatePublication(Publication pub, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return CreatePublication(pub, usedDevice.Id);
    }

    public Publication CreatePublication(Publication pub, string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        var _pub = _deviceApi.CreatePublication(deviceId, pub);
        _state.AddPublication(_pub);
        return _pub;
    }

    public Location UpdateLocation(Location location, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return UpdateLocation(location, usedDevice.Id);
    }

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

    public List<Match> GetMatches(Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return GetMatches(usedDevice.Id);
    }

    public List<Match> GetMatches(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        return _deviceApi.GetMatches(deviceId);
    }

    public MatchMonitor SubscribeMatches(Action<List<Match>> onMatchCallback, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return SubscribeMatches(usedDevice.Id, onMatchCallback);
    }

    public MatchMonitor SubscribeMatches(string deviceId, Action<List<Match>> onMatchCallback)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        List<Match> previous = new List<Match>();
        var monitor = CreateMonitor(deviceId, null);
        _monitors.Add(deviceId, monitor);

        _coroutine.SetupContinuousRoutine(deviceId, () =>
        {
            var m = GetMatches(deviceId);
            var matches = m.Except(previous, new MatchComparer()).ToList();
            onMatchCallback(matches);
            previous = matches;
        });

        return monitor;
    }

    public MatchMonitor SubscribeMatchesWithWS(Action<List<Match>> onMatchCallback, Device device = null)
    {
        var usedDevice = device != null ? device : _state.Device;
        return SubscribeMatchesWithWS(usedDevice.Id, onMatchCallback);
    }

    public MatchMonitor SubscribeMatchesWithWS(string deviceId, Action<List<Match>> onMatchCallback)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        if (_monitors.ContainsKey(deviceId))
        {
            _monitors[deviceId].RefreshSocket(CreateWebsocket(deviceId, onMatchCallback));
        }
        else
        {
            
            var monitor = CreateMonitor(deviceId, CreateWebsocket(deviceId, onMatchCallback));
            _monitors.Add(deviceId, monitor);
        }
        return _monitors[deviceId];
    }

    public List<Subscription> ActiveSubscriptions
    {
        get
        {
            return _state.ActiveSubscriptions;
        }
    }

    public List<Publication> ActivePublications
    {
        get{
            return _state.ActivePublications;
        }
    }

    public void CleanUp()
    {
        foreach (var monitor in _monitors)
        {
            monitor.Value.Stop();
        }

        _monitors.Clear();
        _monitors = null;

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

    private class MatchComparer : IEqualityComparer<Match>
    {
        public bool Equals(Match x, Match y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Match obj)
        {
            return obj.GetHashCode();
        }
    }


    private WebsocketWrapper CreateWebsocket(string deviceId, Action<List<Match>> onMatchCallback)
    {
        List<Match> previous = new List<Match>();
        Action<string> onMatchSocketCallback = matchId =>
        {
            var match = _deviceApi.GetMatch(deviceId, matchId);
            var existing = previous.Find(m => m.Id == match.Id);
            if (existing == null)
            {
                onMatchCallback(new List<Match> { match });
            }

            previous = previous.Concat(new List<Match> { match }).ToList();

        };
        return new WebsocketWrapper(deviceId, _environment, _apiKey, _secured, _pusherPort, onMatchSocketCallback);
    }

    private MatchMonitor CreateMonitor(string deviceId, WebsocketWrapper ws)
    {
        return new MatchMonitor(deviceId, id => _monitors.Remove(id), ws);
    }
}