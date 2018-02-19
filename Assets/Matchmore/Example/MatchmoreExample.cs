using System.Collections;
using System.Collections.Generic;
using Alps.Model;
using UnityEngine;
using UnityEngine.UI;

public class MatchmoreExample : MonoBehaviour
{
    public string apiKey;

    public Text logText;

    public void RunMatchmore()
    {
        Matchmore.Configure(apiKey, useSecuredCommunication: false); // we register the device as the main device
        var mainDevice = Matchmore.Instance.MainDevice;

        //methods interacting with devices by default use the main device, but there are overloads to provide other devices
        Matchmore.Instance.CreatePublication(new Publication
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Properties = new Dictionary<string, object>(){
                {"test", true},
                {"price", 199}
            }
        });
        Matchmore.Instance.CreateSubscription(new Subscription
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Selector = "test = true and price <= 200",
            Pushers = new List<string>() { "ws" }
        });

        //this method is available but we run a coroutine in the background which starts the unity location service and runs this call every time the location was changed
        Matchmore.Instance.UpdateLocation(new Location
        {
            Latitude = 54.414662,
            Longitude = 18.625498
        });

        //query for available matches
        var matches = Matchmore.Instance.GetMatches();
        matches.ForEach(m => {
            logText.text += "\n Got Match " + m.Id + " single call";
        });

        Matchmore.Instance.SubscribeMatches(Matchmore.MatchChannel.Websocket);

        ////or using a lambda
        //Matchmore.Instance.SubscribeMatches(_matches =>
        //{
        //    matches.ForEach(m => {
        //        logText.text += "\n Got Match " + m.Id + " from polling with lambda";
        //    });
        //});

        ////similar api is available for websocket connections
        //Matchmore.Instance.SubscribeMatchesWithWS(HandleMatches);
    }

    //create a delegate which will fire on matches, using polling
    public void HandleMatches(List<Match> matches)
    {
        matches.ForEach(m => {
            logText.text += "\n Got Match " + m.Id + " from polling with delegate";
        });
    }

    public void HandleMatchesWithWS(List<Match> matches)
    {
        matches.ForEach(m => {
            logText.text += "\n Got Match " + m.Id + " from websocket with delegate";
        });
    }

}
