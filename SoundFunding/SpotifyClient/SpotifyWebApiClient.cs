using System;
using System.Collections.Generic;
using System.Web;
using RestSharp;

namespace SoundFunding.SpotifyClient
{
    public class SpotifyWebApiClient
    {
        internal const string ClientID = "28c9a4868be14e358a3256919946c7fd";
        internal const string ClientSecret = "2d687b1ebeba4fcbbdfde3031ab4ec01";

        internal static readonly IEnumerable<string> AuthorizationScopes = new[] {
                                                                                    "playlist-modify-public",
                                                                                    "user-read-private"
                                                                                };

        public SpotifyWebApiClient()
        {
            
        }

        public SpotifyWebApiClient(IAuthenticator authenticator)
        {
            Client.Authenticator = authenticator;
        }

        private RestClient _client;
        
        private RestClient Client
        {
            get { return _client ?? (_client = new RestClient()); }
        }

        public void Authorize(string redirectUri)
        {

        }

        public string GetToken(string code, string redirectUri)
        {
            Client.BaseUrl = new Uri("https://accounts.spotify.com");

            var request = new RestRequest("/api/token", Method.POST);
            request.RequestFormat = DataFormat.Json;

            redirectUri = HttpUtility.UrlEncode(redirectUri);

            request.AddParameter("grant_type", "authorization_code", ParameterType.QueryString);
            request.AddParameter("code", code, ParameterType.QueryString);
            request.AddParameter("redirect_uri", redirectUri, ParameterType.QueryString);
            request.AddParameter("client_id", ClientID, ParameterType.QueryString);
            request.AddParameter("client_secret", ClientSecret, ParameterType.QueryString);

            var response = Client.Execute<dynamic>(request);

            if (response.Data == null)
            {
                //throw new Exception("Call failed to " + request.Resource);
                return response.ErrorException != null ? response.ErrorException.ToString() : response.ErrorMessage;
            }

            return response.Data.access_token;
        }
    }
}