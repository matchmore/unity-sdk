using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class MatchMore
{
    private ApiClient client;
    private DeviceApi deviceApi;
    private MatchmoreState state;
    private GameObject _obj;
    private CoroutineWrapper _coroutine;

    public string Environment { get; set; }

    public string ApiKey { get; set; }

    public string PersistenceFile { get; set; }

    public MatchMore(string apiKey = null, string environment = null)
    {
        if (string.IsNullOrEmpty(environment))
        {
            Environment = "https://35.201.116.232/v5";
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
    }

    private void Init()
    {
        client = new ApiClient(Environment);
        client.AddDefaultHeader("api-key", ApiKey);
        deviceApi = new DeviceApi(client);
        Load();
        _obj = new GameObject("MatchMoreObject");
        _coroutine = _obj.AddComponent<CoroutineWrapper>();
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

        _coroutine.Setup(deviceId, () => {
            var m = GetMatches(deviceId);
            var matches = m.Except(previous, new MatchComparer()).ToList();
            func(matches);
            previous = matches;
        });
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

    ~MatchMore(){
        if(_coroutine != null){
            UnityEngine.Object.Destroy(_coroutine);
        }
        if(_obj != null){
            UnityEngine.Object.Destroy(_obj);
        }
    }
}
