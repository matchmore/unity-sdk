using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// A mobile device is one that potentially moves together with its user and therefore has a geographical location associated with it. 
  /// </summary>
  [DataContract]
  public class MobileDevice : Device {
    /// <summary>
    /// The platform of the device, this can be any string representing the platform type, for instance 'iOS'. 
    /// </summary>
    /// <value>The platform of the device, this can be any string representing the platform type, for instance 'iOS'. </value>
    [DataMember(Name="platform", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "platform")]
    public string Platform { get; set; }

    /// <summary>
    /// The deviceToken is the device push notification token given to this device by the OS, either iOS or Android for identifying the device with push notification services. 
    /// </summary>
    /// <value>The deviceToken is the device push notification token given to this device by the OS, either iOS or Android for identifying the device with push notification services. </value>
    [DataMember(Name="deviceToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceToken")]
    public string DeviceToken { get; set; }

    /// <summary>
    /// Gets or Sets Location
    /// </summary>
    [DataMember(Name="location", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "location")]
    public Location Location { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MobileDevice {\n");
      sb.Append("  Platform: ").Append(Platform).Append("\n");
      sb.Append("  DeviceToken: ").Append(DeviceToken).Append("\n");
      sb.Append("  Location: ").Append(Location).Append("\n");
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
