using NUnit.Framework;
using MatchmorePersistence;
using Alps.Model;
using System.Collections.Generic;

public class StateManagerTest
{
    private StateManager state;
    private string localhost = "localhost";

    [SetUp]
    public void Init()
    {
        state = new StateManager(localhost);
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
        var newState = new StateManager(localhost);
        var persistedSub = newState.ActiveSubscriptions.Find(s => s.Id == "nonsense_id");
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
        var newState = new StateManager(localhost);
        var persistedPub = newState.ActivePublications.Find(p => p.Id == "nonsense_id");
        Assert.NotNull(persistedPub);
    }


    [Test]
    public void Persists_pins()
    {
        var pin = new PinDevice
        {
            Id = "nonsense_id",
            Name = "example pin",
            Location = new Location
            {
                Latitude = 10,
                Longitude = 13
            }
        };

        state.AddPinDevice(pin);
        state = null;
        var newState = new StateManager(localhost);
        var persistedPin = newState.Pins.Find(p => p.Id == "nonsense_id");
        Assert.AreEqual(pin.Id, persistedPin.Id);
        Assert.AreEqual(pin.Name, persistedPin.Name);
        Assert.AreEqual(pin.Location.Latitude, persistedPin.Location.Latitude);
        Assert.AreEqual(pin.Location.Longitude, persistedPin.Location.Longitude);
        Assert.AreEqual(pin.Location.Altitude, persistedPin.Location.Altitude);
    }
}
