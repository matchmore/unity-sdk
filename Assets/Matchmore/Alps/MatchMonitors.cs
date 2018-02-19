using System;
using System.Collections;
using System.Collections.Generic;
using Alps.Api;
using Alps.Model;
using WebSocketSharp;

public interface IMatchMonitor
{
    event EventHandler<MatchReceivedEventArgs> MatchReceived;

    string Id
    {
        get;
    }

    void Stop();
}

public class WebsocketMatchMonitor : IMatchMonitor
{
    private readonly Action<string> _closedCallback;
    private WebSocket _ws;
    private readonly Device _device;
    private DeviceApi _deviceApi;
    private CoroutineWrapper _coroutine;

    public event EventHandler<MatchReceivedEventArgs> MatchReceived;

    public string Id
    {
        get
        {
            return _device.Id;
        }
    }

    public bool IsWebsocketOpened
    {
        get
        {
            return _ws == null ? false : _ws.IsAlive;
        }
    }

    public WebsocketMatchMonitor(Device device, string environment, string apiKey, bool secured, int? pusherPort, DeviceApi deviceApi, CoroutineWrapper coroutine, Action<string> closedCallback)
    {
        _device = device;
        _deviceApi = deviceApi;
        _coroutine = coroutine;
        _closedCallback = closedCallback;
        var worldId = Utils.ExtractWorldId(apiKey);

        UnityEngine.Debug.Log("Starting websocket");

        var protocol = secured ? "wss" : "ws";
        var port = pusherPort == null ? "" : ":" + pusherPort;
        var url = string.Format("{3}://{0}{4}/pusher/{1}/ws/{2}", environment, Matchmore.API_VERSION, _device.Id, protocol, port);
        _ws = new WebSocket(url, "api-key", worldId);

        _ws.OnOpen += (sender, e) => UnityEngine.Debug.Log("WS opened");
        _ws.OnClose += (sender, e) => UnityEngine.Debug.Log("WS closing " + e.Code);
        _ws.OnError += (sender, e) => UnityEngine.Debug.Log("Error in WS " + e.Message);
        _ws.OnMessage += (sender, e) =>
        {
            var data = e.Data;
            if (data == "ping")
            {
                _ws.Send("pong");
            }
            else
            {
                _coroutine.RunOnce(Id, GetMatch(e.Data));
            }
        };
        _ws.Connect();
    }

    private IEnumerator GetMatch(string matchId)
    {
        var match = _deviceApi.GetMatch(_device.Id, matchId);
        OnMatchReceived(new MatchReceivedEventArgs(_device, Matchmore.MatchChannel.Polling, new List<Match>() { match }));
        yield return null;
    }

    private void OnMatchReceived(MatchReceivedEventArgs e)
    {
        EventHandler<MatchReceivedEventArgs> handler = MatchReceived;
        if (handler != null)
        {
            handler(this, e);
        }
    }

    public void Stop()
    {
        if (_ws != null)
            _ws.Close();
        _closedCallback(_device.Id);
    }
}

public class PollingMatchMonitor : IMatchMonitor
{
    private readonly Action<string> _closedCallback;
    private readonly Device _device;
    private DeviceApi _deviceApi;
    private CoroutineWrapper _coroutine;

    public event EventHandler<MatchReceivedEventArgs> MatchReceived;

    public string Id
    {
        get
        {
            return _device.Id;
        }
    }

    public PollingMatchMonitor(Device device, DeviceApi _deviceApi, CoroutineWrapper coroutine, Action<string> closedCallback)
    {
        _device = device;
        _coroutine = coroutine;
        _closedCallback = closedCallback;

        _coroutine.SetupContinuousRoutine(device.Id, () =>
        {
            var matches = _deviceApi.GetMatches(device.Id);
            OnMatchReceived(new MatchReceivedEventArgs(device, Matchmore.MatchChannel.Polling, matches));
        });
    }

    private void OnMatchReceived(MatchReceivedEventArgs e)
    {
        EventHandler<MatchReceivedEventArgs> handler = MatchReceived;
        if (handler != null)
        {
            handler(this, e);
        }
    }

    public void Stop()
    {
        _coroutine.StopContinuousRoutine(_device.Id);
        _closedCallback(_device.Id);
    }
}

public class MatchReceivedEventArgs : EventArgs
{
    private Device _device;
    private Matchmore.MatchChannel _channel;
    private List<Match> _matches;

    public MatchReceivedEventArgs(Device device, Matchmore.MatchChannel channel, List<Match> matches)
    {
        _device = device;
        _channel = channel;
        _matches = matches;
    }
    public Device Device
    {
        get
        {
            return _device;
        }

        private set
        {
            _device = value;
        }
    }

    public Matchmore.MatchChannel Channel
    {
        get
        {
            return _channel;
        }

        private set
        {
            _channel = value;
        }
    }

    public List<Match> Matches
    {
        get
        {
            return _matches;
        }

        private set
        {
            _matches = value;
        }
    }
}