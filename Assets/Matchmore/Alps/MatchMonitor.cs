using System;

public class MatchMonitor
{
    private readonly string _id;
    private readonly Action<string> _closedCallback;
    private WebsocketWrapper _websocket;
    public bool IsWebsocketOpened
    {
        get
        {
            return _websocket == null ? false : _websocket.IsOpened;
        }
    } 

    public MatchMonitor(string id, Action<string> closedCallback, WebsocketWrapper websocket)
    {
        _id = id;
        _closedCallback = closedCallback;
        _websocket = websocket;
    }

    public void RefreshSocket(WebsocketWrapper websocket)
    {
        if (_websocket != null)
            _websocket.Stop();
        _websocket = websocket;
    }

    public void Stop()
    {
        if (_websocket != null)
            _websocket.Stop();
        _closedCallback(_id);
    }
}