using System.Collections.Generic;
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
            var request = new RestRequest("https://accounts.spotify.com/api/token", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("grant_type", "authorization_code", ParameterType.RequestBody);
            request.AddParameter("code", "code", ParameterType.RequestBody);
            request.AddParameter("redirect_uri", redirectUri, ParameterType.RequestBody);

            var response = Client.Execute<dynamic>(request);

            return response.Data.access_token;
        }
    }
}