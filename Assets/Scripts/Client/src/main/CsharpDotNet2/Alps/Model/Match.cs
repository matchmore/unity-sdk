using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alps.Model {

  /// <summary>
  /// An object representing a match between a subscription and a publication.
  /// </summary>
  [DataContract]
  public class Match {
    /// <summary>
    /// The id (UUID) of the match.
    /// </summary>
    /// <value>The id (UUID) of the match.</value>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The timestamp of the match in seconds since Jan 01 1970 (UTC).
    /// </summary>
    /// <value>The timestamp of the match in seconds since Jan 01 1970 (UTC).</value>
    [DataMember(Name="createdAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdAt")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// Gets or Sets Publication
    /// </summary>
    [DataMember(Name="publication", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publication")]
    public Publication Publication { get; set; }

    /// <summary>
    /// Gets or Sets Subscription
    /// </summary>
    [DataMember(Name="subscription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subscription")]
    public Subscription Subscription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Match {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  Publication: ").Append(Publication).Append("\n");
      sb.Append("  Subscription: ").Append(Subscription).Append("\n");
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
