# Alps.Api.DeviceApi

All URIs are relative to *https://api.matchmore.io/v5*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CreateDevice**](DeviceApi.md#createdevice) | **POST** /devices | Create a device
[**CreateLocation**](DeviceApi.md#createlocation) | **POST** /devices/{deviceId}/locations | Create a new location for a device
[**CreatePublication**](DeviceApi.md#createpublication) | **POST** /devices/{deviceId}/publications | Create a publication for a device
[**CreateSubscription**](DeviceApi.md#createsubscription) | **POST** /devices/{deviceId}/subscriptions | Create a subscription for a device
[**DeleteDevice**](DeviceApi.md#deletedevice) | **DELETE** /devices/{deviceId} | Delete an existing device
[**DeletePublication**](DeviceApi.md#deletepublication) | **DELETE** /devices/{deviceId}/publications/{publicationId} | Delete a Publication
[**DeleteSubscription**](DeviceApi.md#deletesubscription) | **DELETE** /devices/{deviceId}/subscriptions/{subscriptionId} | Delete a Subscription
[**GetDevice**](DeviceApi.md#getdevice) | **GET** /devices/{deviceId} | Info about a device
[**GetIBeaconTriples**](DeviceApi.md#getibeacontriples) | **GET** /devices/IBeaconTriples | Get IBeacons triples for all registered devices
[**GetMatch**](DeviceApi.md#getmatch) | **GET** /devices/{deviceId}/matches/{matchId} | Get match for the device by its id
[**GetMatches**](DeviceApi.md#getmatches) | **GET** /devices/{deviceId}/matches | Get matches for the device
[**GetPublication**](DeviceApi.md#getpublication) | **GET** /devices/{deviceId}/publications/{publicationId} | Info about a publication on a device
[**GetPublications**](DeviceApi.md#getpublications) | **GET** /devices/{deviceId}/publications | Get all publications for a device
[**GetSubscription**](DeviceApi.md#getsubscription) | **GET** /devices/{deviceId}/subscriptions/{subscriptionId} | Info about a subscription on a device
[**GetSubscriptions**](DeviceApi.md#getsubscriptions) | **GET** /devices/{deviceId}/subscriptions | Get all subscriptions for a device
[**TriggerProximityEvents**](DeviceApi.md#triggerproximityevents) | **POST** /devices/{deviceId}/proximityEvents | Trigger the proximity event between a device and a ranged BLE iBeacon
[**UpdateDevice**](DeviceApi.md#updatedevice) | **PATCH** /devices/{deviceId} | Updates name or/and device token for existing device


<a name="createdevice"></a>
# **CreateDevice**
> Device CreateDevice (Device device)

Create a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class CreateDeviceExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var device = new Device(); // Device | The device to be created.

            try
            {
                // Create a device
                Device result = apiInstance.CreateDevice(device);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.CreateDevice: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **device** | [**Device**](Device.md)| The device to be created. | 

### Return type

[**Device**](Device.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

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
            
            var apiInstance = new DeviceApi();
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
                Debug.Print("Exception when calling DeviceApi.CreateLocation: " + e.Message );
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
            
            var apiInstance = new DeviceApi();
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
                Debug.Print("Exception when calling DeviceApi.CreatePublication: " + e.Message );
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

<a name="createsubscription"></a>
# **CreateSubscription**
> Subscription CreateSubscription (string deviceId, Subscription subscription)

Create a subscription for a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class CreateSubscriptionExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device. 
            var subscription = new Subscription(); // Subscription | Subscription to create on a device. 

            try
            {
                // Create a subscription for a device
                Subscription result = apiInstance.CreateSubscription(deviceId, subscription);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.CreateSubscription: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device.  | 
 **subscription** | [**Subscription**](Subscription.md)| Subscription to create on a device.  | 

### Return type

[**Subscription**](Subscription.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletedevice"></a>
# **DeleteDevice**
> void DeleteDevice (string deviceId)

Delete an existing device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class DeleteDeviceExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Delete an existing device
                apiInstance.DeleteDevice(deviceId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.DeleteDevice: " + e.Message );
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

void (empty response body)

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
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var publicationId = publicationId_example;  // string | The id (UUID) of the subscription.

            try
            {
                // Delete a Publication
                apiInstance.DeletePublication(deviceId, publicationId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.DeletePublication: " + e.Message );
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

<a name="deletesubscription"></a>
# **DeleteSubscription**
> void DeleteSubscription (string deviceId, string subscriptionId)

Delete a Subscription



### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class DeleteSubscriptionExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var subscriptionId = subscriptionId_example;  // string | The id (UUID) of the subscription.

            try
            {
                // Delete a Subscription
                apiInstance.DeleteSubscription(deviceId, subscriptionId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.DeleteSubscription: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **subscriptionId** | **string**| The id (UUID) of the subscription. | 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdevice"></a>
# **GetDevice**
> Device GetDevice (string deviceId)

Info about a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetDeviceExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Info about a device
                Device result = apiInstance.GetDevice(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetDevice: " + e.Message );
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

[**Device**](Device.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getibeacontriples"></a>
# **GetIBeaconTriples**
> IBeaconTriples GetIBeaconTriples ()

Get IBeacons triples for all registered devices

Keys in map are device UUIDs and values are IBeacon triples. In model you can see example values \"property1\" \"property2\" \"property3\" instead of random UUIDs this is generated by OpenApi document browser

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetIBeaconTriplesExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();

            try
            {
                // Get IBeacons triples for all registered devices
                IBeaconTriples result = apiInstance.GetIBeaconTriples();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetIBeaconTriples: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**IBeaconTriples**](IBeaconTriples.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmatch"></a>
# **GetMatch**
> Match GetMatch (string userId, string deviceId, string matchId)

Get match for the device by its id

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetMatchExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var userId = userId_example;  // string | The id (UUID) of the user of the device.
            var deviceId = deviceId_example;  // string | The id (UUID) of the user device.
            var matchId = matchId_example;  // string | The id (UUID) of the match.

            try
            {
                // Get match for the device by its id
                Match result = apiInstance.GetMatch(userId, deviceId, matchId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetMatch: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **string**| The id (UUID) of the user of the device. | 
 **deviceId** | **string**| The id (UUID) of the user device. | 
 **matchId** | **string**| The id (UUID) of the match. | 

### Return type

[**Match**](Match.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmatches"></a>
# **GetMatches**
> Matches GetMatches (string deviceId)

Get matches for the device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetMatchesExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get matches for the device
                Matches result = apiInstance.GetMatches(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetMatches: " + e.Message );
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

[**Matches**](Matches.md)

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
            
            var apiInstance = new DeviceApi();
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
                Debug.Print("Exception when calling DeviceApi.GetPublication: " + e.Message );
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
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get all publications for a device
                Publications result = apiInstance.GetPublications(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetPublications: " + e.Message );
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

<a name="getsubscription"></a>
# **GetSubscription**
> Subscription GetSubscription (string deviceId, string subscriptionId)

Info about a subscription on a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetSubscriptionExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var subscriptionId = subscriptionId_example;  // string | The id (UUID) of the subscription.

            try
            {
                // Info about a subscription on a device
                Subscription result = apiInstance.GetSubscription(deviceId, subscriptionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetSubscription: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **subscriptionId** | **string**| The id (UUID) of the subscription. | 

### Return type

[**Subscription**](Subscription.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsubscriptions"></a>
# **GetSubscriptions**
> Subscriptions GetSubscriptions (string deviceId)

Get all subscriptions for a device

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class GetSubscriptionsExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get all subscriptions for a device
                Subscriptions result = apiInstance.GetSubscriptions(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.GetSubscriptions: " + e.Message );
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

[**Subscriptions**](Subscriptions.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="triggerproximityevents"></a>
# **TriggerProximityEvents**
> ProximityEvent TriggerProximityEvents (string deviceId, ProximityEvent proximityEvent)

Trigger the proximity event between a device and a ranged BLE iBeacon

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class TriggerProximityEventsExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var proximityEvent = new ProximityEvent(); // ProximityEvent | The proximity event to be created for the device.

            try
            {
                // Trigger the proximity event between a device and a ranged BLE iBeacon
                ProximityEvent result = apiInstance.TriggerProximityEvents(deviceId, proximityEvent);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.TriggerProximityEvents: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **deviceId** | **string**| The id (UUID) of the device. | 
 **proximityEvent** | [**ProximityEvent**](ProximityEvent.md)| The proximity event to be created for the device. | 

### Return type

[**ProximityEvent**](ProximityEvent.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatedevice"></a>
# **UpdateDevice**
> Device UpdateDevice (DeviceUpdate device)

Updates name or/and device token for existing device

Token can be only updated for mobile devices.

### Example
```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class UpdateDeviceExample
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var device = new DeviceUpdate(); // DeviceUpdate | The device update description.

            try
            {
                // Updates name or/and device token for existing device
                Device result = apiInstance.UpdateDevice(device);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.UpdateDevice: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **device** | [**DeviceUpdate**](DeviceUpdate.md)| The device update description. | 

### Return type

[**Device**](Device.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

