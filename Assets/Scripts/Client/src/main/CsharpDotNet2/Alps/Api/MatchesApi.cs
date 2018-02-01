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
    public interface IMatchesApi
    {
        /// <summary>
        /// Get match for the device by its id 
        /// </summary>
        /// <param name="userId">The id (UUID) of the user of the device.</param>
        /// <param name="deviceId">The id (UUID) of the user device.</param>
        /// <param name="matchId">The id (UUID) of the match.</param>
        /// <returns>Match</returns>
        Match GetMatch (string userId, string deviceId, string matchId);
        /// <summary>
        /// Get matches for the device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param>
        /// <returns>Matches</returns>
        Matches GetMatches (string deviceId);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MatchesApi : IMatchesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MatchesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MatchesApi(String basePath)
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
        /// Get match for the device by its id 
        /// </summary>
        /// <param name="userId">The id (UUID) of the user of the device.</param> 
        /// <param name="deviceId">The id (UUID) of the user device.</param> 
        /// <param name="matchId">The id (UUID) of the match.</param> 
        /// <returns>Match</returns>            
        public Match GetMatch (string userId, string deviceId, string matchId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetMatch");
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetMatch");
            
            // verify the required parameter 'matchId' is set
            if (matchId == null) throw new ApiException(400, "Missing required parameter 'matchId' when calling GetMatch");
            
    
            var path = "/devices/{deviceId}/matches/{matchId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
path = path.Replace("{" + "deviceId" + "}", ApiClient.ParameterToString(deviceId));
path = path.Replace("{" + "matchId" + "}", ApiClient.ParameterToString(matchId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetMatch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMatch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Match) ApiClient.Deserialize(response.Content, typeof(Match), response.Headers);
        }
    
        /// <summary>
        /// Get matches for the device 
        /// </summary>
        /// <param name="deviceId">The id (UUID) of the device.</param> 
        /// <returns>Matches</returns>            
        public Matches GetMatches (string deviceId)
        {
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling GetMatches");
            
    
            var path = "/devices/{deviceId}/matches";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetMatches: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMatches: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Matches) ApiClient.Deserialize(response.Content, typeof(Matches), response.Headers);
        }
    
    }
}
