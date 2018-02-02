using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class Location {
    /// <summary>
    /// The timestamp of the location creation in seconds since Jan 01 1970 (UTC). 
    /// </summary>
    /// <value>The timestamp of the location creation in seconds since Jan 01 1970 (UTC). </value>
    [DataMember(Name="createdAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdAt")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// The latitude of the device in degrees, for instance '46.5333' (Lausanne, Switzerland). 
    /// </summary>
    /// <value>The latitude of the device in degrees, for instance '46.5333' (Lausanne, Switzerland). </value>
    [DataMember(Name="latitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "latitude")]
    public double? Latitude { get; set; }

    /// <summary>
    /// The longitude of the device in degrees, for instance '6.6667' (Lausanne, Switzerland). 
    /// </summary>
    /// <value>The longitude of the device in degrees, for instance '6.6667' (Lausanne, Switzerland). </value>
    [DataMember(Name="longitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "longitude")]
    public double? Longitude { get; set; }

    /// <summary>
    /// The altitude of the device in meters, for instance '495.0' (Lausanne, Switzerland). 
    /// </summary>
    /// <value>The altitude of the device in meters, for instance '495.0' (Lausanne, Switzerland). </value>
    [DataMember(Name="altitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitude")]
    public double? Altitude { get; set; }

    /// <summary>
    /// The horizontal accuracy of the location, measured on a scale from '0.0' to '1.0', '1.0' being the most accurate. If this value is not specified then the default value of '1.0' is used. 
    /// </summary>
    /// <value>The horizontal accuracy of the location, measured on a scale from '0.0' to '1.0', '1.0' being the most accurate. If this value is not specified then the default value of '1.0' is used. </value>
    [DataMember(Name="horizontalAccuracy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "horizontalAccuracy")]
    public double? HorizontalAccuracy { get; set; }

    /// <summary>
    /// The vertical accuracy of the location, measured on a scale from '0.0' to '1.0', '1.0' being the most accurate. If this value is not specified then the default value of '1.0' is used. 
    /// </summary>
    /// <value>The vertical accuracy of the location, measured on a scale from '0.0' to '1.0', '1.0' being the most accurate. If this value is not specified then the default value of '1.0' is used. </value>
    [DataMember(Name="verticalAccuracy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "verticalAccuracy")]
    public double? VerticalAccuracy { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Location {\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  Latitude: ").Append(Latitude).Append("\n");
      sb.Append("  Longitude: ").Append(Longitude).Append("\n");
      sb.Append("  Altitude: ").Append(Altitude).Append("\n");
      sb.Append("  HorizontalAccuracy: ").Append(HorizontalAccuracy).Append("\n");
      sb.Append("  VerticalAccuracy: ").Append(VerticalAccuracy).Append("\n");
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
