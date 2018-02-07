using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Alps.Model;
using System.Collections.Generic;

public class MatchMoreTest
{
    public const string API_KEY = "eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhbHBzIiwic3ViIjoiZDY5MDVhYmEtNmEzOS00ZmU5LTg3NGYtNzM4ZGZlNDc1YjhkIiwiYXVkIjpbIlB1YmxpYyJdLCJuYmYiOjE1MTgwMDY4ODQsImlhdCI6MTUxODAwNjg4NCwianRpIjoiMSJ9.qP_tlXTeqZu-rG6ZNYuSYpbfMna6251WbwqCAh3xSYOMiXnca6WJkwEqcBytjYv3FdWtgGxt_yHXBj0uQ21vhQ";
    public const string ENVIRONMENT = "localhost";

    [UnityTest]
    public IEnumerator Add_device_pub_sub_and_get_match_via_poll()
    {
        var matchMore = new MatchMore(API_KEY, ENVIRONMENT, secured: false);
        Device subDevice = CreateSubscribingMobileDevice(matchMore);
        Subscription sub;
        Publication pub;
        SetupMatch(matchMore, subDevice, out sub, out pub);

        Match match = null;
        for (int i = 10 - 1; i >= 0; i--)
        {
            var matches = matchMore.GetMatches(subDevice.Id);
            match = matches.Find(m => m.Publication.Id == pub.Id && m.Subscription.Id == sub.Id);
            if (match != null)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(3);
            }
        }

        Assert.IsNotNull(match);
    }

    [UnityTest]
    public IEnumerator Add_device_pub_sub_and_get_match_via_subsciption()
    {
        var matchMore = new MatchMore(API_KEY, ENVIRONMENT, secured: false);
        Device subDevice = CreateSubscribingMobileDevice(matchMore);
        Subscription sub;
        Publication pub;
        SetupMatch(matchMore, subDevice, out sub, out pub);

        Match match = null;

        matchMore.SubscribeMatches(subDevice, matches =>
        {
            match = matches.Find(m => m.Publication.Id == pub.Id && m.Subscription.Id == sub.Id);
        });

        for (int i = 10 - 1; i >= 0; i--)
        {
            if (match != null)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(3);
            }
        }

        Assert.IsNotNull(match);
    }

    [UnityTest]
    public IEnumerator Add_device_pub_sub_and_get_match_via_web_socket()
    {
        var matchMore = new MatchMore(API_KEY, ENVIRONMENT, secured: false);
        Device subDevice = CreateSubscribingMobileDevice(matchMore);

        matchMore.StartWebSocket(subDevice.Id);
        yield return new WaitForSeconds(3);

        Subscription sub;
        Publication pub;
        SetupMatch(matchMore, subDevice, out sub, out pub);


        Match match = null;

        matchMore.SubscribeMatchesWithWS(subDevice, matches =>
        {
            match = matches.Find(m => m.Publication.Id == pub.Id && m.Subscription.Id == sub.Id);
        });

        for (int i = 10 - 1; i >= 0; i--)
        {
            if (match != null)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(3);
            }
        }

        Assert.IsNotNull(match);
    }

    private static Device CreateSubscribingMobileDevice(MatchMore matchMore)
    {
        Device subDevice = matchMore.CreateDevice(new MobileDevice
        {
            Name = "Subscriber",
            DeviceToken = ""
        });
        Assert.NotNull(subDevice);
        Assert.NotNull(subDevice.Id);
        return subDevice;
    }

    private static void SetupMatch(MatchMore matchMore, Device subDevice, out Subscription sub, out Publication pub)
    {
 
        matchMore.MainDevice = subDevice;

        var pubDevice = matchMore.CreateDevice(new MobileDevice
        {
            Name = "Publisher"
        });


        Assert.NotNull(pubDevice);
        Assert.NotNull(pubDevice.Id);

        sub = matchMore.CreateSubscription(subDevice, new Subscription
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Selector = "test = true and price <= 200",
            Pushers = new List<string>(){"ws"}
        });
        Assert.NotNull(sub);
        Assert.NotNull(sub.Id);

        pub = matchMore.CreatePublication(pubDevice, new Publication
        {
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Properties = new Dictionary<string, object>(){
                {"test", true},
                {"price", 199}
            }
        });
        Assert.NotNull(pub);
        Assert.NotNull(pub.Id);

        matchMore.UpdateLocation(subDevice, new Location
        {
            Latitude = 54.414662,
            Longitude = 18.625498
        });

        matchMore.UpdateLocation(pubDevice, new Location
        {
            Latitude = 54.414662,
            Longitude = 18.625498
        });
    }
}
