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

    public WebsocketMatchMonitor(Device device, Matchmore.Config config, DeviceApi deviceApi, CoroutineWrapper coroutine, Action<string> closedCallback)
    {
        if (device == null || string.IsNullOrEmpty(device.Id))
        {
            throw new ArgumentException("Device null or invalid id");
        }

        _device = device;
        _deviceApi = deviceApi;
        _coroutine = coroutine;
        _closedCallback = closedCallback;
        var worldId = Utils.ExtractWorldId(config.ApiKey);

        MatchmoreLogger.Debug("Starting websocket for device {0}", device.Id);

        var protocol = config.UseSecuredCommunication ? "wss" : "ws";
        var port = config.PusherPort == null ? "" : ":" + config.PusherPort;
        var url = string.Format("{3}://{0}{4}/pusher/{1}/ws/{2}", config.Environment, Matchmore.API_VERSION, _device.Id, protocol, port);
        _ws = new WebSocket(url, "api-key", worldId);

        _ws.OnOpen += (sender, e) => MatchmoreLogger.Debug("WS opened for device {0}", device.Id);
        _ws.OnClose += (sender, e) => MatchmoreLogger.Debug("WS closing {0} for device {1}", e.Code, device.Id);
        _ws.OnError += (sender, e) => MatchmoreLogger.Debug("Error in WS {0} for device {1}", e.Message, device.Id);
        _ws.OnMessage += (sender, e) =>
        {
            var data = e.Data;
            if (data == "ping")
            {
                _ws.Send("pong");
            }
            else
            {
                _coroutine.RunOnce(Id, GetMatch(data));
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
        if (device == null || string.IsNullOrEmpty(device.Id))
        {
            throw new ArgumentException("Device null or invalid id");
        }

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
