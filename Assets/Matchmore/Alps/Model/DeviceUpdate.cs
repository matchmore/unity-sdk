using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// Describes update of device, it allows to change name of device and device token (only in case of mobile devices)
  /// </summary>
  [DataContract]
  public class DeviceUpdate {
    /// <summary>
    /// New device name (optional)
    /// </summary>
    /// <value>New device name (optional)</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Token used for pushing matches. The token needs to be prefixed with `apns://` or `fcm://` dependent on the device or channel the match should be pushed with
    /// </summary>
    /// <value>Token used for pushing matches. The token needs to be prefixed with `apns://` or `fcm://` dependent on the device or channel the match should be pushed with</value>
    [DataMember(Name="deviceToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceToken")]
    public string DeviceToken { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DeviceUpdate {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  DeviceToken: ").Append(DeviceToken).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
