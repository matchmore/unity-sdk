using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Alps.Model;
using MatchmoreExtensions;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using System.Threading;

public class ExtensionTest
{
    [Test]
    public void Pub_Seconds_remaining(){
        var pub = new Publication
        {
            Id = "id",
            CreatedAt = (long?)TimeUtil.Now(),
            Duration = 30
        };

        Assert.LessOrEqual(pub.SecondsRemaining(), 30);
    }

    [Test]
    public void Pub_is_alive()
    {
        var pub = new Publication
        {
            Id = "id",
            CreatedAt = (long?)TimeUtil.Now(),
            Duration = 1
        };
        Assert.IsTrue(pub.IsAlive());
        Thread.Sleep(2000);
        Assert.IsFalse(pub.IsAlive());
    }

    [Test]
    public void Sub_Seconds_remaining()
    {
        var sub = new Subscription
        {
            Id = "id",
            CreatedAt = (long?)TimeUtil.Now(),
            Duration = 30
        };

        Assert.LessOrEqual(sub.SecondsRemaining(), 30);
    }

    [Test]
    public void Sub_is_alive()
    {
        var sub = new Subscription
        {
            Id = "id",
            CreatedAt = (long?)TimeUtil.Now(),
            Duration = 1
        };
        Assert.IsTrue(sub.IsAlive());
        Thread.Sleep(2000);
        Assert.IsFalse(sub.IsAlive());
    }
}
