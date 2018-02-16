using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System;

public class WebsocketWrapper
{
    private WebSocket _ws;

    public bool IsOpened
    {
        get
        {
            return _ws == null ? false : _ws.IsAlive;
        }
    }

    public WebsocketWrapper(string deviceId, string environment, string apiKey, bool secured, int? pusherPort, Action<string> onMatchCallback)
    {
        var worldId = Utils.ExtractWorldId(apiKey);

        UnityEngine.Debug.Log("Starting websocket");

        var protocol = secured ? "wss" : "ws";
        var port = pusherPort == null ? "" : ":" + pusherPort;
        var url = string.Format("{3}://{0}{4}/pusher/{1}/ws/{2}", environment, Matchmore.API_VERSION, deviceId, protocol, port);
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
                if (onMatchCallback != null)
                    onMatchCallback(data);
            }
        };
        _ws.Connect();
    }

    public void Stop()
    {
        _ws.Close();
    }
}
