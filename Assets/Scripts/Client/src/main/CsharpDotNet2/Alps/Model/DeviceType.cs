using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model
{

    /// <summary>
    /// A device might be either virtual like a pin device or physical like a mobile phone or iBeacon device. 
    /// </summary>

    public class DeviceType
    {
        public const string Mobile = "MobileDevice";
        public const string Pin = "PinDevice";
        public const string IBeacon = "IBeaconDevice";
    }
}
