using UnityEngine;
using System.Linq;
using Alps.Model;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MatchmoreExtensions;
using System;

namespace MatchmorePersistence
{
    public partial class StateManager
    {
        private State _state;
        private string _env;
        string _persistenceFile;

        public string PersistenceFile
        {
            get
            {
                return _persistenceFile;
            }
        }

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

        public StateManager(string env, string persistenceFile = null)
        {
            _env = env;
            _persistenceFile = string.IsNullOrEmpty(persistenceFile) ? "state.data" : persistenceFile;
            Load();
        }

        public void WipeData()
        {
            File.Delete(PersistencePath);
        }

        public void Load()
        {
            if (!File.Exists(PersistencePath))
            {
                _state = new State
                {
                    Environment = _env
                };
                return;
            }

            if (new FileInfo(PersistencePath).Length == 0)
            {
                _state = new State()
                {
                    Environment = _env  
                };
                return;
            }

            using (StreamReader file = File.OpenText(PersistencePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                var state = (State)serializer.Deserialize(file, typeof(State));
                if (IsCorrectEnv(state))
                {
                    _state = state;
                }
                else
                {
                    throw new InvalidOperationException("State belongs to a wrong environment, please wipe the data using the WipeData() method");
                }
            }
        }

        private bool IsCorrectEnv(State state)
        {
            return state.Environment == _env;
        }

        public void Save()
        {
            if (_state.IsDirty())
            {
                using (StreamWriter file = File.CreateText(PersistencePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var json = JsonConvert.SerializeObject(_state, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    serializer.Serialize(file, _state);
                    _state.MarkAsClean();
                }
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

        public void PruneDead()
        {
            _state.Publications = _state.Publications.Where(pub => pub.IsAlive()).ToList();

            _state.Subscriptions = _state.Subscriptions.Where(sub => sub.IsAlive()).ToList();
        }
    }
}