using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Alps.Model {

  /// <summary>
  /// A device might be either virtual like a pin device or physical like a mobile phone or iBeacon device. 
  /// </summary>
  [DataContract]
  public class Device {
    /// <summary>
    /// The id (UUID) of the device.
    /// </summary>
    /// <value>The id (UUID) of the device.</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The timestamp of the device's creation in seconds since Jan 01 1970 (UTC). 
    /// </summary>
    /// <value>The timestamp of the device's creation in seconds since Jan 01 1970 (UTC). </value>
    [DataMember(Name="createdAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdAt")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// The timestamp of the device's creation in seconds since Jan 01 1970 (UTC). 
    /// </summary>
    /// <value>The timestamp of the device's creation in seconds since Jan 01 1970 (UTC). </value>
    [DataMember(Name="updatedAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "updatedAt")]
    public long? UpdatedAt { get; set; }

    /// <summary>
    /// Optional device groups, one device can belong to multiple groups, grops are string that can be max 25 characters long and contains letters numbers or underscores
    /// </summary>
    /// <value>Optional device groups, one device can belong to multiple groups, grops are string that can be max 25 characters long and contains letters numbers or underscores</value>
    [DataMember(Name="group", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "group")]
    public List<string> Group { get; set; }

    /// <summary>
    /// The name of the device.
    /// </summary>
    /// <value>The name of the device.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets DeviceType
    /// </summary>
    [DataMember(Name="deviceType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceType")]
    public string DeviceType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Device {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
      sb.Append("  Group: ").Append(Group).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  DeviceType: ").Append(DeviceType).Append("\n");
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
