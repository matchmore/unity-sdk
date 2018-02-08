using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Matchmore.Persistence;
using Alps.Model;
using System.Collections.Generic;

public class StateManagerTest
{
    private StateManager state;

    [SetUp]
    public void Init()
    {
        state = new StateManager();
    }

    [TearDown]
    public void CleanData()
    {
        if (state != null)
            state.WipeData();
        state = null;
    }

    [Test]
    public void Persists_sub()
    {
        state.AddSubscription(new Subscription
        {
            Id = "nonsense_id",
            Topic = "Unity",
            Duration = 60,
            Range = 100,
            Selector = "test = true and price <= 200"
        });
        state = null;
        var newState = new StateManager();
        var persistedSub = newState.ActiveSubscriptions.Find(f => f.Id == "nonsense_id");
        Assert.NotNull(persistedSub);
    }


    [Test]
    public void Persists_pub()
    {
        state.AddPublication(new Publication
        {
            Id = "nonsense_id",
            Topic = "Unity",
            Duration = 30,
            Range = 100,
            Properties = new Dictionary<string, object>(){
                {"test", true},
                {"price", 199}
            }
        });
        state = null;
        var newState = new StateManager();
        var persistedPub = newState.ActivePublications.Find(f => f.Id == "nonsense_id");
        Assert.NotNull(persistedPub);
    }
}
