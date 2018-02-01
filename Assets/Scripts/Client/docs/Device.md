# Alps.Model.Device
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** | The id (UUID) of the device. | 
**CreatedAt** | **long?** | The timestamp of the device&#39;s creation in seconds since Jan 01 1970 (UTC).  | 
**UpdatedAt** | **long?** | The timestamp of the device&#39;s creation in seconds since Jan 01 1970 (UTC).  | [optional] 
**Group** | **List&lt;string&gt;** | Optional device groups, one device can belong to multiple groups, grops are string that can be max 25 characters long and contains letters numbers or underscores | [optional] 
**Name** | **string** | The name of the device. | [optional] [default to ""]
**DeviceType** | **DeviceType** |  | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

