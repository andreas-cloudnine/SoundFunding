using System.Web;
using RestSharp;

namespace SoundFunding.SpotifyClient
{
    public class SpotifyAuthenticator : IAuthenticator
    {
        private const string SpotifyTokenSessionKey = "SpotifyToken";
        
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var token = GetTokenFromSession();

            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", "Bearer  " + token);
            }
        }

        public static void AddTokenToSession(string token)
        {
            HttpContext.Current.Session[SpotifyTokenSessionKey] = token;
        }

        public static string GetTokenFromSession()
        {
            return HttpContext.Current.Session[SpotifyTokenSessionKey] as string;
        }
    }
}