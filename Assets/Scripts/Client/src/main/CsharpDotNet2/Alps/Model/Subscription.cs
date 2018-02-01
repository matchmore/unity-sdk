using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// A subscription can be seen as a JMS subscription extended with the notion of geographical zone. The zone again being defined as circle with a center at the given location and a range around that location. 
  /// </summary>
  [DataContract]
  public class Subscription {
    /// <summary>
    /// The id (UUID) of the subscription.
    /// </summary>
    /// <value>The id (UUID) of the subscription.</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The timestamp of the subscription creation in seconds since Jan 01 1970 (UTC). 
    /// </summary>
    /// <value>The timestamp of the subscription creation in seconds since Jan 01 1970 (UTC). </value>
    [DataMember(Name="createdAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdAt")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// The id (UUID) of the world that contains device to attach a subscription to.
    /// </summary>
    /// <value>The id (UUID) of the world that contains device to attach a subscription to.</value>
    [DataMember(Name="worldId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "worldId")]
    public string WorldId { get; set; }

    /// <summary>
    /// The id (UUID) of the device to attach a subscription to.
    /// </summary>
    /// <value>The id (UUID) of the device to attach a subscription to.</value>
    [DataMember(Name="deviceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceId")]
    public string DeviceId { get; set; }

    /// <summary>
    /// The topic of the subscription. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic. 
    /// </summary>
    /// <value>The topic of the subscription. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic. </value>
    [DataMember(Name="topic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "topic")]
    public string Topic { get; set; }

    /// <summary>
    /// This is an expression to filter the publications. For instance 'job='developer'' will allow matching only with publications containing a 'job' key with a value of 'developer'. 
    /// </summary>
    /// <value>This is an expression to filter the publications. For instance 'job='developer'' will allow matching only with publications containing a 'job' key with a value of 'developer'. </value>
    [DataMember(Name="selector", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "selector")]
    public string Selector { get; set; }

    /// <summary>
    /// The range of the subscription in meters. This is the range around the device holding the subscription in which matches with publications can be triggered. 
    /// </summary>
    /// <value>The range of the subscription in meters. This is the range around the device holding the subscription in which matches with publications can be triggered. </value>
    [DataMember(Name="range", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "range")]
    public double? Range { get; set; }

    /// <summary>
    /// The duration of the subscription in seconds. If set to '-1' the subscription will live forever and if set to '0' it will be instant at the time of subscription. 
    /// </summary>
    /// <value>The duration of the subscription in seconds. If set to '-1' the subscription will live forever and if set to '0' it will be instant at the time of subscription. </value>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public double? Duration { get; set; }

    /// <summary>
    /// When match will occurs, they will be notified on these provided URI(s) address(es) in the pushers array. 
    /// </summary>
    /// <value>When match will occurs, they will be notified on these provided URI(s) address(es) in the pushers array. </value>
    [DataMember(Name="pushers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushers")]
    public List<string> Pushers { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Subscription {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  WorldId: ").Append(WorldId).Append("\n");
      sb.Append("  DeviceId: ").Append(DeviceId).Append("\n");
      sb.Append("  Topic: ").Append(Topic).Append("\n");
      sb.Append("  Selector: ").Append(Selector).Append("\n");
      sb.Append("  Range: ").Append(Range).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  Pushers: ").Append(Pushers).Append("\n");
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
