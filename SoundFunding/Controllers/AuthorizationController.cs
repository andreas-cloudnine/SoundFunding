using System.Threading.Tasks;
using System.Web.Mvc;
using SoundFunding.SpotifyClient;

namespace SoundFunding.Controllers
{
    public class AuthorizationController : Controller
    {
        private const string RedirectUri = "http://soundfunding.azurewebsites.net/authorization/callback/";

        public ActionResult Login()
        {
            var url = "https://accounts.spotify.com/authorize?client_id={client_id}&response_type={response_type}&redirect_uri={redirect_uri}&scope={scope}";

            url = url.Replace("{client_id}", SpotifyWebApiClient.ClientID);
            url = url.Replace("{response_type}", "code");
            url = url.Replace("{redirect_uri}", Url.Encode(RedirectUri));
            url = url.Replace("{scope}", string.Join(" ", SpotifyWebApiClient.AuthorizationScopes));

            return Redirect(url);
        }

        public async Task<ActionResult> Callback(string code, string state, string error)
        {
            SpotifyWebAPI.Authentication.ClientId = SpotifyWebApiClient.ClientID;
            SpotifyWebAPI.Authentication.ClientSecret = SpotifyWebApiClient.ClientSecret;
            SpotifyWebAPI.Authentication.RedirectUri = RedirectUri;

            var response = await SpotifyWebAPI.Authentication.GetAccessToken(code);

            var token = response.AccessToken;
            
            SpotifyAuthenticator.AddTokenToSession(token);

            return Content(token);
        }
    }
}