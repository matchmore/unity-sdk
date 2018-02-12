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
    public interface IPublicationApi
    {
        /// <summary>
        /// Create a publication for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <param name="publication">Publication to create on a device. </param>
        /// <returns>Publication</returns>
        Publication CreatePublication (string deviceId, Publication publication);
        /// <summary>
        /// Delete a Publication 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <param name="publicationId">The id (UUID) of the subscription.</param>
        /// <returns></returns>
        void DeletePublication (string deviceId, string publicationId);
        /// <summary>
        /// Info about a publication on a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <param name="publicationId">The id (UUID) of the publication.</param>
        /// <returns>Publication</returns>
        Publication GetPublication (string deviceId, string publicationId);
        /// <summary>
        /// Get all publications for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <returns>Publications</returns>
        Publications GetPublications (string deviceId);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PublicationApi : IPublicationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PublicationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PublicationApi(String basePath)
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
        /// Create a publication for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <param name="publication">Publication to create on a device. </param> 
        /// <returns>Publication</returns>            
        public Publication CreatePublication (string deviceId, Publication publication)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling CreatePublication");
            
            // verify the required parameter 'publication' is set
            if (publication == null) throw new ApiException(400, "Missing required parameter 'publication' when calling CreatePublication");
            
    
            var path = "/devices/{deviceId}/publications";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(publication); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreatePublication: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreatePublication: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Publication) ApiClient.Deserialize(response.Content, typeof(Publication), response.Headers);
        }
    
        /// <summary>
        /// Delete a Publication 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <param name="publicationId">The id (UUID) of the subscription.</param> 
        /// <returns></returns>            
        public void DeletePublication (string deviceId, string publicationId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling DeletePublication");
            
            // verify the required parameter 'publicationId' is set
            if (publicationId == null) throw new ApiException(400, "Missing required parameter 'publicationId' when calling DeletePublication");
            
    
            var path = "/devices/{deviceId}/publications/{publicationId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
path = path.Replace("{" + "publicationId" + "}", ApiClient.ParameterToString(publicationId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePublication: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePublication: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Info about a publication on a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <param name="publicationId">The id (UUID) of the publication.</param> 
        /// <returns>Publication</returns>            
        public Publication GetPublication (string deviceId, string publicationId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetPublication");
            
            // verify the required parameter 'publicationId' is set
            if (publicationId == null) throw new ApiException(400, "Missing required parameter 'publicationId' when calling GetPublication");
            
    
            var path = "/devices/{deviceId}/publications/{publicationId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
path = path.Replace("{" + "publicationId" + "}", ApiClient.ParameterToString(publicationId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPublication: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPublication: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Publication) ApiClient.Deserialize(response.Content, typeof(Publication), response.Headers);
        }
    
        /// <summary>
        /// Get all publications for a device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <returns>Publications</returns>            
        public Publications GetPublications (string deviceId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetPublications");
            
    
            var path = "/devices/{deviceId}/publications";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPublications: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPublications: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Publications) ApiClient.Deserialize(response.Content, typeof(Publications), response.Headers);
        }
    
    }
}
