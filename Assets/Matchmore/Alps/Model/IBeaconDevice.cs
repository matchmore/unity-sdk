using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// An iBeacon device represents an Apple conform iBeacon announcing its presence via Bluetooth advertising packets. 
  /// </summary>
  [DataContract]
  public class IBeaconDevice : Device {
    /// <summary>
    /// The UUID of the beacon, the purpose is to distinguish iBeacons in your network, from all other beacons in networks outside your control. 
    /// </summary>
    /// <value>The UUID of the beacon, the purpose is to distinguish iBeacons in your network, from all other beacons in networks outside your control. </value>
    [DataMember(Name="proximityUUID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "proximityUUID")]
    public string ProximityUUID { get; set; }

    /// <summary>
    /// Major values are intended to identify and distinguish a group. 
    /// </summary>
    /// <value>Major values are intended to identify and distinguish a group. </value>
    [DataMember(Name="major", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "major")]
    public int? Major { get; set; }

    /// <summary>
    /// Minor values are intended to identify and distinguish an individual. 
    /// </summary>
    /// <value>Minor values are intended to identify and distinguish an individual. </value>
    [DataMember(Name="minor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minor")]
    public int? Minor { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IBeaconDevice {\n");
      sb.Append("  ProximityUUID: ").Append(ProximityUUID).Append("\n");
      sb.Append("  Major: ").Append(Major).Append("\n");
      sb.Append("  Minor: ").Append(Minor).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public  new string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
