# Alps.Model.Subscription
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** | The id (UUID) of the subscription. | 
**CreatedAt** | **long?** | The timestamp of the subscription creation in seconds since Jan 01 1970 (UTC).  | 
**WorldId** | **string** | The id (UUID) of the world that contains device to attach a subscription to. | 
**DeviceId** | **string** | The id (UUID) of the device to attach a subscription to. | 
**Topic** | **string** | The topic of the subscription. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic.  | 
**Selector** | **string** | This is an expression to filter the publications. For instance &#39;job&#x3D;&#39;developer&#39;&#39; will allow matching only with publications containing a &#39;job&#39; key with a value of &#39;developer&#39;.  | 
**Range** | **double?** | The range of the subscription in meters. This is the range around the device holding the subscription in which matches with publications can be triggered.  | 
**Duration** | **double?** | The duration of the subscription in seconds. If set to &#39;-1&#39; the subscription will live forever and if set to &#39;0&#39; it will be instant at the time of subscription.  | 
**Pushers** | **List&lt;string&gt;** | When match will occurs, they will be notified on these provided URI(s) address(es) in the pushers array.  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

