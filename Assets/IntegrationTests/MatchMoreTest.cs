using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Alps.Model;
using System.Collections.Generic;

public class MatchMoreTest
{


    [UnityTest]
    public IEnumerator Add_device_pub_sub_and_get_match()
    {
        var matchMore = new MatchMore("eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhbHBzIiwic3ViIjoiZTcxMmE1YjEtMDNkMi00NmFlLWE1NDEtOGFjZmFiMGJjM2M0IiwiYXVkIjpbIlB1YmxpYyJdLCJuYmYiOjE1MTY4MjA4MzIsImlhdCI6MTUxNjgyMDgzMiwianRpIjoiMSJ9.SyRjVl-4yss3oUUiZ1GPwl9uEt76H3npwDiuISSCmbcu-qCDUmnzfpMOXG7I7hqJUcCZoFxRINDMDFUdTACKQw");

        var subDevice = matchMore.CreateDevice(new MobileDevice
        {
            Name = "Subscriber",
            DeviceToken = ""
        });

        Assert.NotNull(subDevice);
        Assert.NotNull(subDevice.Id);

        var pubDevice = matchMore.CreateDevice(new MobileDevice
        {
            Name = "Publisher",
            DeviceToken = "",
        });

        Assert.NotNull(pubDevice);
        Assert.NotNull(pubDevice.Id);

        var sub = matchMore.CreateSubscription(subDevice, new Subscription
        {
            Topic = "Unity",
            Duration = 30,
            Range = 1000,
            Selector = "test = true and price <= 200",
            Pushers = new List<string>()
        });

        Assert.NotNull(sub);
        Assert.NotNull(sub.Id);

        var pub = matchMore.CreatePublication(pubDevice, new Publication
        {
            Topic = "Unity",
            Duration = 30,
            Range = 1000,
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

}
