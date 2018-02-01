# Alps.Api.SubscriptionApi

All URIs are relative to *https://api.matchmore.io/v5*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CreateSubscription**](SubscriptionApi.md#createsubscription) | **POST** /devices/{deviceId}/subscriptions | Create a subscription for a device
[**DeleteSubscription**](SubscriptionApi.md#deletesubscription) | **DELETE** /devices/{deviceId}/subscriptions/{subscriptionId} | Delete a Subscription
[**GetSubscription**](SubscriptionApi.md#getsubscription) | **GET** /devices/{deviceId}/subscriptions/{subscriptionId} | Info about a subscription on a device
[**GetSubscriptions**](SubscriptionApi.md#getsubscriptions) | **GET** /devices/{deviceId}/subscriptions | Get all subscriptions for a device


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
            
            var apiInstance = new SubscriptionApi();
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
                Debug.Print("Exception when calling SubscriptionApi.CreateSubscription: " + e.Message );
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
            
            var apiInstance = new SubscriptionApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.
            var subscriptionId = subscriptionId_example;  // string | The id (UUID) of the subscription.

            try
            {
                // Delete a Subscription
                apiInstance.DeleteSubscription(deviceId, subscriptionId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SubscriptionApi.DeleteSubscription: " + e.Message );
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
            
            var apiInstance = new SubscriptionApi();
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
                Debug.Print("Exception when calling SubscriptionApi.GetSubscription: " + e.Message );
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
            
            var apiInstance = new SubscriptionApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get all subscriptions for a device
                Subscriptions result = apiInstance.GetSubscriptions(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SubscriptionApi.GetSubscriptions: " + e.Message );
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

