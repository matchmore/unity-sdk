# Alps.Model.Publication
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** | The id (UUID) of the publication. | 
**CreatedAt** | **long?** | The timestamp of the publication creation in seconds since Jan 01 1970 (UTC).  | 
**WorldId** | **string** | The id (UUID) of the world that contains device to attach a publication to. | 
**DeviceId** | **string** | The id (UUID) of the device to attach a publication to. | 
**Topic** | **string** | The topic of the publication. This will act as a first match filter. For a subscription to be able to match a publication they must have the exact same topic.  | 
**Range** | **double?** | The range of the publication in meters. This is the range around the device holding the publication in which matches with subscriptions can be triggered.  | 
**Duration** | **double?** | The duration of the publication in seconds. If set to &#39;-1&#39; the publication will live forever and if set to &#39;0&#39; it will be instant at the time of publication.  | 
**Properties** | **Object** | The dictionary of key, value pairs. Allowed values are number, boolean, string and array of afformentioned types | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

