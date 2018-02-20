using Alps.Model;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MatchmorePersistence
{
    public partial class StateManager
    {
        [DataContract]
        private class State
        {
            private bool _isDirty;
            private string environment;
            private Device device;
            private List<Subscription> subscriptions;
            private List<Publication> publications;
            private List<PinDevice> pins;

            [DataMember(Name = "env", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "env")]
            public string Environment
            {
                get
                {
                    return environment;
                }

                set
                {
                    _isDirty = true;
                    environment = value;
                }
            }

            [DataMember(Name = "device", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "device")]
            public Device Device
            {
                get
                {
                    return device;
                }

                set
                {
                    _isDirty = true;
                    device = value;
                }
            }

            [DataMember(Name = "subscriptions", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "subscriptions")]
            public List<Subscription> Subscriptions
            {
                get
                {
                    return subscriptions;
                }

                set
                {
                    _isDirty = true;
                    subscriptions = value;
                }
            }

            [DataMember(Name = "publications", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "publications")]
            public List<Publication> Publications
            {
                get
                {
                    return publications;
                }

                set
                {
                    _isDirty = true;
                    publications = value;
                }
            }

            [DataMember(Name = "pins", EmitDefaultValue = false)]
            [JsonProperty(PropertyName = "pins")]
            public List<PinDevice> Pins
            {
                get
                {
                    return pins;
                }

                set
                {
                    _isDirty = true;
                    pins = value;
                }
            }

            public State()
            {
                Subscriptions = new List<Subscription>();
                Publications = new List<Publication>();
                Pins = new List<PinDevice>();
            }

            public bool IsDirty()
            {
                return _isDirty;
            }

            public void MarkAsClean()
            {
                _isDirty = false;
            }
        }
    }
   
}