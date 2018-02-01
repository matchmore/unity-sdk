# Alps.Api.MatchesApi

All URIs are relative to *https://api.matchmore.io/v5*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetMatch**](MatchesApi.md#getmatch) | **GET** /devices/{deviceId}/matches/{matchId} | Get match for the device by its id
[**GetMatches**](MatchesApi.md#getmatches) | **GET** /devices/{deviceId}/matches | Get matches for the device


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
            
            var apiInstance = new MatchesApi();
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
                Debug.Print("Exception when calling MatchesApi.GetMatch: " + e.Message );
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
            
            var apiInstance = new MatchesApi();
            var deviceId = deviceId_example;  // string | The id (UUID) of the device.

            try
            {
                // Get matches for the device
                Matches result = apiInstance.GetMatches(deviceId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MatchesApi.GetMatches: " + e.Message );
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

