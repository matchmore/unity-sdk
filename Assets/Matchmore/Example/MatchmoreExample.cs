using System.Collections;
using System.Collections.Generic;
using Alps.Model;
using UnityEngine;
using UnityEngine.UI;
using MatchmoreExtensions;
using System.Net;

public class MatchmoreExample : MonoBehaviour
{
    [TextArea]
    public string apiKey;

    public Text logText;

    public void RunMatchmore()
    {
        StartCoroutine(MatchmoreCoroutine());
        LogLine("Started example");
    }

    public void Start()
    {
        //only works if you put the DDLs
        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var config = Matchmore.Config.WithApiKey(apiKey);

        config.UseSecuredCommunication = true;
        Matchmore.Configure(config);
        Matchmore.Instance.WipeData();
    }

    public IEnumerator MatchmoreCoroutine()
    {
        yield return null;
        // we register the device as the main device
        var mainDevice = Matchmore.Instance.MainDevice;

        //methods interacting with devices by default use the main device, but there are overloads to provide other devices
        var pub = Matchmore.Instance.CreatePublication(new Publication
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Properties = new Dictionary<string, object>(){
                {"test", true},
                {"price", 199}
            }
        });

        var sub = Matchmore.Instance.CreateSubscription(new Subscription
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Selector = "test = true and price <= 200",
            Pushers = new List<string>() { "ws" }
        });

        foreach (var _pub in Matchmore.Instance.ActivePublications)
        {
            LogLine(string.Format("Pub {0} existings for another {1} seconds", _pub.Id, _pub.SecondsRemaining()));
        }

        foreach (var _sub in Matchmore.Instance.ActiveSubscriptions)
        {
            LogLine(string.Format("Sub {0} existing for another {1} seconds", _sub.Id, _sub.SecondsRemaining()));
        }

        //this method is available but we run a coroutine in the background which starts the unity location service and runs this call every time the location was changed
        Matchmore.Instance.UpdateLocation(new Location
        {
            Latitude = 54.414662,
            Longitude = 18.625498
        });

        //query for available matches via a single request
        var matches = Matchmore.Instance.GetMatches();
        matches.ForEach(m =>
        {
            LogLine("Got Match " + m.Id + " single call");
        });

        //creates a monitor on a single device which has an event handler(by default the main one)
        var socketMonitor = Matchmore.Instance.SubscribeMatches(Matchmore.MatchChannel.websocket);

        //creates a monitor on a single device which has an event handler(by default the main one)
        var pollingMonitor = Matchmore.Instance.SubscribeMatches(Matchmore.MatchChannel.polling);

        //this event handler fires only for a single device
        socketMonitor.MatchReceived += (sender, e) => {
            LogLine(string.Format("Received match {0} from device {1} with channel {2} directly from monitor", e.Matches[0].Id, e.Device.Id, e.Channel));
        };

        pollingMonitor.MatchReceived += (sender, e) => {
            LogLine(string.Format("Received match {0} from device {1} with channel {2} directly from monitor", e.Matches[0].Id, e.Device.Id, e.Channel));
        };

        //this event handler fires for all devices created
        Matchmore.Instance.MatchReceived += (sender, e) => {
            LogLine(string.Format("Received match {0} from device {1} with channel {2}", e.Matches[0].Id, e.Device.Id, e.Channel));
        };
    }

    private void LogLine(string line)
    {
        logText.text += "\n" + line;
    }

}
