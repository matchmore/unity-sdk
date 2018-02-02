using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// A publication can be seen as a JavaMessagingService (JMS) publication extended with the notion of a geographical zone. The zone is defined as circle with a center at the given location and a range around that location. 
  /// </summary>
  [DataContract]
  public class Publication {
    /// <summary>
    /// The id (UUID) of the publication.
    /// </summary>
    /// <value>The id (UUID) of the publication.</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The timestamp of the publication creation in seconds since Jan 01 1970 (UTC). 
    /// </summary>
    /// <value>The timestamp of the publication creation in seconds since Jan 01 1970 (UTC). </value>
    [DataMember(Name="createdAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdAt")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// The id (UUID) of the world that contains device to attach a publication to.
    /// </summary>
    /// <value>The id (UUID) of the world that contains device to attach a publication to.</value>
    [DataMember(Name="worldId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "worldId")]
    public string WorldId { get; set; }

    /// <summary>
    /// The id (UUID) of the device to attach a publication to.
    /// </summary>
    /// <value>The id (UUID) of the device to attach a publication to.</value>
    [DataMember(Name="deviceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceId")]
    public string DeviceId { get; set; }

    /// <summary>
    /// The topic of the publication. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic. 
    /// </summary>
    /// <value>The topic of the publication. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic. </value>
    [DataMember(Name="topic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "topic")]
    public string Topic { get; set; }

    /// <summary>
    /// The range of the publication in meters. This is the range around the device holding the publication in which matches with subscriptions can be triggered. 
    /// </summary>
    /// <value>The range of the publication in meters. This is the range around the device holding the publication in which matches with subscriptions can be triggered. </value>
    [DataMember(Name="range", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "range")]
    public double? Range { get; set; }

    /// <summary>
    /// The duration of the publication in seconds. If set to '-1' the publication will live forever and if set to '0' it will be instant at the time of publication. 
    /// </summary>
    /// <value>The duration of the publication in seconds. If set to '-1' the publication will live forever and if set to '0' it will be instant at the time of publication. </value>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public double? Duration { get; set; }

    /// <summary>
    /// The dictionary of key, value pairs. Allowed values are number, boolean, string and array of afformentioned types
    /// </summary>
    /// <value>The dictionary of key, value pairs. Allowed values are number, boolean, string and array of afformentioned types</value>
    [DataMember(Name="properties", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "properties")]
    public Dictionary<string, Object> Properties { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Publication {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  WorldId: ").Append(WorldId).Append("\n");
      sb.Append("  DeviceId: ").Append(DeviceId).Append("\n");
      sb.Append("  Topic: ").Append(Topic).Append("\n");
      sb.Append("  Range: ").Append(Range).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  Properties: ").Append(Properties).Append("\n");
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
