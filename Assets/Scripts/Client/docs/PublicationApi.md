# Alps.Api.PublicationApi

All URIs are relative to *https://api.matchmore.io/v5*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CreatePublication**](PublicationApi.md#createpublication) | **POST** /devices/{deviceId}/publications | Create a publication for a device
[**DeletePublication**](PublicationApi.md#deletepublication) | **DELETE** /devices/{deviceId}/publications/{publicationId} | Delete a Publication
[**GetPublication**](PublicationApi.md#getpublication) | **GET** /devices/{deviceId}/publications/{publicationId} | Info about a publication on a device
[**GetPublications**](PublicationApi.md#getpublications) | **GET** /devices/{deviceId}/publications | Get all publications for a device


<a name="createpublication"></a>
# **CreatePublication**
> Publication CreatePublication (string deviceId, Publication publication)

Create a publication for a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class CreatePublicationExample
    {
        public void main()
        {
            
            var apiInstance = new PublicationApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var publication = new Publication(); // Publication | Publication to create on a device. 

            try
            {
                // Create a publication for a device
                Publication result = apiInstance.CreatePublication(deviceId, publication);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PublicationApi.CreatePublication: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **publication** | [**Publication**](Publication.md)| Publication to create on a device.  | 

### Return type

[**Publication**](Publication.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletepublication"></a>
# **DeletePublication**
> void DeletePublication (string deviceId, string publicationId)

Delete a Publication



### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class DeletePublicationExample
    {
        public void main()
        {
            
            var apiInstance = new PublicationApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var publicationId = publicationId_example;  // string | The id (UUID) of the subscription.

            try
            {
                // Delete a Publication
                apiInstance.DeletePublication(deviceId, publicationId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PublicationApi.DeletePublication: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **publicationId** | **string**| The id (UUID) of the subscription. | 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpublication"></a>
# **GetPublication**
> Publication GetPublication (string deviceId, string publicationId)

Info about a publication on a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetPublicationExample
    {
        public void main()
        {
            
            var apiInstance = new PublicationApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var publicationId = publicationId_example;  // string | The id (UUID) of the publication.

            try
            {
                // Info about a publication on a device
                Publication result = apiInstance.GetPublication(deviceId, publicationId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PublicationApi.GetPublication: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **publicationId** | **string**| The id (UUID) of the publication. | 

### Return type

[**Publication**](Publication.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpublications"></a>
# **GetPublications**
> Publications GetPublications (string deviceId)

Get all publications for a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetPublicationsExample
    {
        public void main()
        {
            
            var apiInstance = new PublicationApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get all publications for a device
                Publications result = apiInstance.GetPublications(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PublicationApi.GetPublications: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 

### Return type

[**Publications**](Publications.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

