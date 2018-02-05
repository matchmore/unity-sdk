using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;

public class MatchMore
{
    public const string API_VERSION = "v5";
    private ApiClient client;
    private DeviceApi deviceApi;
    private MatchmoreState state;
    private GameObject _obj;
    private CoroutineWrapper _coroutine;
    private WebSocket _ws;

    public string Environment { get; set; }

    public string ApiKey { get; set; }

    public string PersistenceFile { get; set; }


    public string ApiUrl
    {
        get
        {
            return String.Format("https://{0}/{1}", Environment, API_VERSION);
        }
    }

    public MatchMore(string apiKey = null, string environment = null)
    {
        if (string.IsNullOrEmpty(environment))
        {
            Environment = "35.201.116.232";
        }
        else
        {
            Environment = environment;
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            ApiKey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhbHBzIiwic3ViIjoiZTcxMmE1YjEtMDNkMi00NmFlLWE1NDEtOGFjZmFiMGJjM2M0IiwiYXVkIjpbIlB1YmxpYyJdLCJuYmYiOjE1MTY4MjA4MzIsImlhdCI6MTUxNjgyMDgzMiwianRpIjoiMSJ9.SyRjVl-4yss3oUUiZ1GPwl9uEt76H3npwDiuISSCmbcu-qCDUmnzfpMOXG7I7hqJUcCZoFxRINDMDFUdTACKQw";
        }
        else
        {
            ApiKey = apiKey;
        }

        PersistenceFile = "matchmore.dat";
        Init();
        Load();
    }

    private void Init()
    {
        client = new ApiClient(ApiUrl);
        client.AddDefaultHeader("api-key", ApiKey);
        deviceApi = new DeviceApi(client);
        _obj = new GameObject("MatchMoreObject");
        _coroutine = _obj.AddComponent<CoroutineWrapper>();
    }

    private void StartWebSocket()
    {
        var url = String.Format("ws://{0}/pusher/{1}/ws/{2}", Environment, API_VERSION, state.Device.Id);
        _ws = new WebSocket(url, new string[] { "Sec-WebSocket-Protocol", "api-key", ApiKey })
        {
            EmitOnPing = true
        };
     
        _ws.OnOpen += (sender, e) => UnityEngine.MonoBehaviour.print("On Open " + e);
        _ws.OnClose += (sender, e) => UnityEngine.MonoBehaviour.print("On Close " + e.Code);
        _ws.OnError += (sender, e) => UnityEngine.MonoBehaviour.print("On Error " + e.Message);
        _ws.OnMessage += (sender, e) => UnityEngine.MonoBehaviour.print("On Message " + e.Data);
         _ws.Connect();
    }

    private string PersistencePath
    {
        get
        {
            return Application.persistentDataPath + "/" + PersistenceFile;
        }
    }

    private void Load()
    {
        var file = PersistencePath;
        var fs = new FileStream(file, FileMode.OpenOrCreate);
        if (fs.Length == 0)
        {
            state = new MatchmoreState();
        }
        else
        {
            var formatter = new BinaryFormatter();
            state = formatter.Deserialize(fs) as MatchmoreState;
        }

        fs.Close();
    }

    private void Save()
    {
        var formatter = new BinaryFormatter();
        var fs = new FileStream(PersistenceFile, FileMode.Truncate);
        formatter.Serialize(fs, state);
        fs.Close();
    }

    public Device CreateDevice(Device device)
    {
        if (state == null)
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

            if (string.IsNullOrEmpty(mobileDevice.Platform))
            {
                mobileDevice.Platform = Application.platform.ToString();
            }

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

        var resultingDevice = deviceApi.CreateDevice(createdDevice);

        state.Device = resultingDevice;

        return resultingDevice;
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

        var createdSub = deviceApi.CreateSubscription(deviceId, sub);
        return createdSub;
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

        var createdPub = deviceApi.CreatePublication(deviceId, pub);
        return createdPub;
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

        return deviceApi.CreateLocation(deviceId, location);
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

        return deviceApi.GetMatches(deviceId);
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

        List<Match> previous = new List<Match>();

        StartWebSocket();
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

    ~MatchMore()
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
}
