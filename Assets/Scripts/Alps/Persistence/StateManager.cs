using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Alps.Model;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Matchmore.Persistence
{
    public class StateManager
    {
        [DataContract]
        private class State
        {
            [DataMember(Name = "device", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "device")]
            public Device Device { get; set; }

            [DataMember(Name = "subscriptions", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "subscriptions")]
            public List<Subscription> Subscriptions { get; set; }

            [DataMember(Name = "publications", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "publications")]
            public List<Publication> Publications { get; set; }

            [DataMember(Name = "pins", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "pins")]
            public List<PinDevice> Pins { get; set; }

            public State()
            {
                Subscriptions = new List<Subscription>();
                Publications = new List<Publication>();
                Pins = new List<PinDevice>();
            }
        }

        private State _state;

        public string PersistenceFile { get; set; }

        public Device Device
        {
            get
            {
                if (_state == null)
                {
                    return null;
                }
                else
                {
                    return _state.Device;
                }
            }
            set
            {
                if (_state == null)
                {
                    _state = new State();
                }

                _state.Device = value;
            }
        }

        public string PersistencePath
        {
            get
            {
                return Application.persistentDataPath + Path.DirectorySeparatorChar + PersistenceFile;
            }
        }

        public List<Subscription> ActiveSubscriptions
        {
            get
            {
                return _state.Subscriptions;
            }
        }

        public List<Publication> ActivePublications
        {
            get
            {
                return _state.Publications;
            }
        }

        public List<PinDevice> Pins
        {
            get
            {
                return _state.Pins;
            }
        }

        public StateManager()
        {
            PersistenceFile = "state.data";
            Load();
        }

        public void WipeData()
        {
            //File.Delete(PersistencePath);
        }

        public void Load()
        {
            if (!File.Exists(PersistencePath))
            {
                _state = new State();
                return;
            }

            if (new FileInfo(PersistencePath).Length == 0)
            {
                _state = new State();
                return;
            }

            using (StreamReader file = File.OpenText(PersistencePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                _state = (State)serializer.Deserialize(file, typeof(State));
            }
        }

        public void Save()
        {
            using (StreamWriter file = File.CreateText(PersistencePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                var json = JsonConvert.SerializeObject(_state, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                serializer.Serialize(file, _state);
            }
        }

        public void AddSubscription(Subscription sub)
        {
            _state.Subscriptions.Add(sub);
            Save();
        }

        public void AddPublication(Publication pub)
        {
            _state.Publications.Add(pub);
            Save();
        }

        public void AddPinDevice(PinDevice pinDevice)
        {
            _state.Pins.Add(pinDevice);
            Save();
        }

        public void CheckDuration()
        {
            var now = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            _state.Publications = _state.Publications.Where(pub =>
            {
                var miliDelta = now - pub.CreatedAt;
                return miliDelta / 1000f < pub.Duration;
            }).ToList();

            _state.Subscriptions = _state.Subscriptions.Where(sub =>
            {
                var miliDelta = now - sub.CreatedAt;
                return miliDelta / 1000f < sub.Duration;
            }).ToList();
        }
    }

}