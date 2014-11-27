using System;
using System.Text;
using RestSharp;

namespace SoundFunding.SpotifyClient
{
    public class BasicAuthenticator : IAuthenticator
    {
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var bytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", SpotifyWebApiClient.ClientID, SpotifyWebApiClient.ClientSecret));
            string base64 = Convert.ToBase64String(bytes);

            request.AddHeader("Authorization", "Basic " + base64);
        }
    }
}