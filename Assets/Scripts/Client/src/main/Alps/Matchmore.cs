using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using System.Text;
using Newtonsoft.Json;
using Matchmore.Persistence;

public class MatchMore
{
    public const string API_VERSION = "v5";
    private ApiClient _client;
    private DeviceApi _deviceApi;
    private StateManager _state;
    private GameObject _obj;
    private CoroutineWrapper _coroutine;
    private WebSocket _ws;
    private string _environment;
    private string _apiKey;
    private bool _secured;
    private string _worldId;
    private bool _websocketStarted = false;

    public Device MainDevice
    {
        get
        {
            return _state.Device;
        }
        set
        {
            _state.Device = value;
        }
    }

    public string ApiUrl
    {
        get
        {
            var protocol = _secured ? "https" : "http";
            return String.Format("{2}://{0}:9000/{1}", _environment, API_VERSION, protocol);
        }
    }


    public class ApiKeyObject
    {
        public string Sub { get; set; }
    }

    public MatchMore(string apiKey, string environment, bool secured = true, bool websocket = false, string worldId = null)
    {
        if (string.IsNullOrEmpty(environment))
        {
            throw new ArgumentException("Environment null or empty");
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("Api key null or empty");
        }

        if (string.IsNullOrEmpty(worldId))
        {
            ApiKeyObject deserializedApiKey = ExtractWorldId(apiKey);
            _worldId = deserializedApiKey.Sub;
        }
        else
        {
            _worldId = worldId;
        }

        _environment = environment;
        _apiKey = apiKey;
        _secured = secured;

        Init();

        _state = new StateManager();

        _coroutine.Setup("persistence", _state.CheckDuration);

        if (websocket)
        {
            StartWebSocket();
        }
    }

    private void Init()
    {
        _client = new ApiClient(ApiUrl);
        _client.AddDefaultHeader("api-key", _apiKey);
        _deviceApi = new DeviceApi(_client);
        _obj = new GameObject("MatchMoreObject_" + UnityEngine.Random.Range(0, 100));
        _coroutine = _obj.AddComponent<CoroutineWrapper>();
    }

    public void StartWebSocket(string forDeviceId = null)
    {
        if (_websocketStarted)
            return;

        var deviceId = string.IsNullOrEmpty(forDeviceId) ? _state.Device.Id : forDeviceId;
        var protocol = _secured ? "wss" : "ws";
        var url = String.Format("{3}://{0}:9001/pusher/{1}/ws/{2}", _environment, API_VERSION, deviceId, protocol);
        _ws = new WebSocket(url, "api-key", _worldId);

        //_ws.OnOpen += (sender, e) => UnityEngine.MonoBehaviour.print("On Open " + e);
        //_ws.OnClose += (sender, e) => UnityEngine.MonoBehaviour.print("On Close " + e.Code);
        //_ws.OnError += (sender, e) => UnityEngine.MonoBehaviour.print("On Error " + e.Message);
        _ws.OnMessage += (sender, e) =>
        {
            if (e.Data == "ping")
            {
                _ws.Send("pong");
            }
        };
        _ws.Connect();
        _websocketStarted = true;
    }

    public Device CreateDevice(Device device)
    {
        if (_state == null)
        {
            throw new InvalidOperationException("Persistence wasn't setup!!!");
        }

        Device createdDevice = null;
        if (string.IsNullOrEmpty(device.Name))
        {
            throw new ArgumentException("Missing name");
        }

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

        return _deviceApi.CreateDevice(createdDevice);
    }

    public Subscription CreateSubscription(Device device, Subscription sub)
    {
        return CreateSubscription(device.Id, sub);
    }
    public Subscription CreateSubscription(string deviceId, Subscription sub)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }


        var _sub = _deviceApi.CreateSubscription(deviceId, sub);
        _state.AddSubscription(_sub);
        return _sub;
    }

    public Publication CreatePublication(Device device, Publication pub)
    {
        return CreatePublication(device.Id, pub);
    }

    public Publication CreatePublication(string deviceId, Publication pub)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        var _pub = _deviceApi.CreatePublication(deviceId, pub);
        _state.AddPublication(_pub);
        return _pub;
    }

    public Location UpdateLocation(Device device, Location location)
    {
        return UpdateLocation(device.Id, location);
    }

    public Location UpdateLocation(string deviceId, Location location)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        if (location.Altitude == null)
            location.Altitude = 0;

        return _deviceApi.CreateLocation(deviceId, location);
    }

    public List<Match> GetMatches(Device device)
    {
        return GetMatches(device.Id);
    }

    public List<Match> GetMatches(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        return _deviceApi.GetMatches(deviceId);
    }

    public void SubscribeMatches(Device device, Action<List<Match>> func)
    {
        SubscribeMatches(device.Id, func);
    }

    public void SubscribeMatches(string deviceId, Action<List<Match>> func)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        List<Match> previous = new List<Match>();

        _coroutine.Setup(deviceId, () =>
        {
            var m = GetMatches(deviceId);
            var matches = m.Except(previous, new MatchComparer()).ToList();
            func(matches);
            previous = matches;
        });
    }

    public void SubscribeMatchesWithWS(Device device, Action<List<Match>> func)
    {
        SubscribeMatchesWithWS(device.Id, func);
    }

    public void SubscribeMatchesWithWS(string deviceId, Action<List<Match>> func)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Device Id null or empty");
        }

        StartWebSocket(deviceId);

        List<Match> previous = new List<Match>();
        _ws.OnMessage += (sender, e) =>
        {
            var match = _deviceApi.GetMatch(deviceId, e.Data);
            var existing = previous.Find(m => m.Id == match.Id);
            if (existing == null)
            {
                func(new List<Match> { match });
            }

            previous = previous.Concat(new List<Match> { match }).ToList();

        };
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

    public List<Subscription> ActiveSubscriptions
    {
        get
        {
            return _state.ActiveSubscriptions;
        }
    }


    public void CleanUp()
    {
        if (_ws != null)
        {
            _ws.Close();
        }
        if (_coroutine != null)
        {
            UnityEngine.Object.Destroy(_coroutine);
        }
        if (_obj != null)
        {
            UnityEngine.Object.Destroy(_obj);
        }
    }

    private static ApiKeyObject ExtractWorldId(string apiKey)
    {
        var subjectData = Convert.FromBase64String(apiKey.Split('.')[1]);
        var subject = Encoding.UTF8.GetString(subjectData);
        var deserializedApiKey = JsonConvert.DeserializeObject<ApiKeyObject>(subject);
        return deserializedApiKey;
    }
}
