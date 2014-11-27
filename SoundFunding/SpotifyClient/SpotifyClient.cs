using RestSharp;

namespace SoundFunding.SpotifyClient
{
    public class SpotifyClient
    {
        public SpotifyClient(IAuthenticator authenticator)
        {
            Client.Authenticator = authenticator;
        }

        private RestClient _client;
        
        protected RestClient Client
        {
            get { return _client ?? (_client = new RestClient()); }
        }
    }
}