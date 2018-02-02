using System;
using System.Collections.Generic;
using RestSharp;
using Alps.Client;
using Alps.Model;

namespace Alps.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISubscriptionApi
    {
        /// <summary>
        /// Create a subscription for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device. </param>
        /// <param name="subscription">Subscription to create on a device. </param>
        /// <returns>Subscription</returns>
        Subscription CreateSubscription (string deviceId, Subscription subscription);
        /// <summary>
        /// Delete a Subscription 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <param name="subscriptionId">The id (UUID) of the subscription.</param>
        /// <returns></returns>
        void DeleteSubscription (string deviceId, string subscriptionId);
        /// <summary>
        /// Info about a subscription on a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <param name="subscriptionId">The id (UUID) of the subscription.</param>
        /// <returns>Subscription</returns>
        Subscription GetSubscription (string deviceId, string subscriptionId);
        /// <summary>
        /// Get all subscriptions for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <returns>Subscriptions</returns>
        Subscriptions GetSubscriptions (string deviceId);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SubscriptionApi : ISubscriptionApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public SubscriptionApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SubscriptionApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        /// Create a subscription for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device. </param> 
        /// <param name="subscription">Subscription to create on a device. </param> 
        /// <returns>Subscription</returns>            
        public Subscription CreateSubscription (string deviceId, Subscription subscription)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling CreateSubscription");
            
            // verify the required parameter 'subscription' is set
            if (subscription == null) throw new ApiException(400, "Missing required parameter 'subscription' when calling CreateSubscription");
            
    
            var path = "/devices/{deviceId}/subscriptions";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(subscription); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateSubscription: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateSubscription: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Subscription) ApiClient.Deserialize(response.Content, typeof(Subscription), response.Headers);
        }
    
        /// <summary>
        /// Delete a Subscription 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <param name="subscriptionId">The id (UUID) of the subscription.</param> 
        /// <returns></returns>            
        public void DeleteSubscription (string deviceId, string subscriptionId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling DeleteSubscription");
            
            // verify the required parameter 'subscriptionId' is set
            if (subscriptionId == null) throw new ApiException(400, "Missing required parameter 'subscriptionId' when calling DeleteSubscription");
            
    
            var path = "/devices/{deviceId}/subscriptions/{subscriptionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
path = path.Replace("{" + "subscriptionId" + "}", ApiClient.ParameterToString(subscriptionId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSubscription: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSubscription: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Info about a subscription on a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <param name="subscriptionId">The id (UUID) of the subscription.</param> 
        /// <returns>Subscription</returns>            
        public Subscription GetSubscription (string deviceId, string subscriptionId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetSubscription");
            
            // verify the required parameter 'subscriptionId' is set
            if (subscriptionId == null) throw new ApiException(400, "Missing required parameter 'subscriptionId' when calling GetSubscription");
            
    
            var path = "/devices/{deviceId}/subscriptions/{subscriptionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
path = path.Replace("{" + "subscriptionId" + "}", ApiClient.ParameterToString(subscriptionId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSubscription: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSubscription: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Subscription) ApiClient.Deserialize(response.Content, typeof(Subscription), response.Headers);
        }
    
        /// <summary>
        /// Get all subscriptions for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <returns>Subscriptions</returns>            
        public Subscriptions GetSubscriptions (string deviceId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetSubscriptions");
            
    
            var path = "/devices/{deviceId}/subscriptions";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSubscriptions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSubscriptions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Subscriptions) ApiClient.Deserialize(response.Content, typeof(Subscriptions), response.Headers);
        }
    
    }
}
