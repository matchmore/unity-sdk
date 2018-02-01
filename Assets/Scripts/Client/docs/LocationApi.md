# Alps.Api.LocationApi

All URIs are relative to *https://api.matchmore.io/v5*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CreateLocation**](LocationApi.md#createlocation) | **POST** /devices/{deviceId}/locations | Create a new location for a device


<a name="createlocation"></a>
# **CreateLocation**
> Location CreateLocation (string deviceId, Location location)

Create a new location for a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class CreateLocationExample
    {
        public void main()
        {
            
            var apiInstance = new LocationApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var location = new Location(); // Location | Location to create for a device. 

            try
            {
                // Create a new location for a device
                Location result = apiInstance.CreateLocation(deviceId, location);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling LocationApi.CreateLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **location** | [**Location**](Location.md)| Location to create for a device.  | 

### Return type

[**Location**](Location.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

