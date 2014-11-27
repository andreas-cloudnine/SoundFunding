using System.Web.Mvc;
using SoundFunding.SpotifyClient;

namespace SoundFunding.Controllers
{
    public class AuthorizationController : Controller
    {
        private const string RedirectUri = "http://soundfunding.azurewebsites.net/authorization/callback";

        public ActionResult Login()
        {
            var url = "https://accounts.spotify.com/authorize?client_id={client_id}&response_type={response_type}&redirect_uri={redirect_uri}&scope={scope}";

            url = url.Replace("{client_id}", SpotifyWebApiClient.ClientID);
            url = url.Replace("{response_type}", "code");
            url = url.Replace("{redirect_uri}", Url.Encode(RedirectUri));
            url = url.Replace("{scope}", string.Join(" ", SpotifyWebApiClient.AuthorizationScopes));

            return Redirect(url);
        }

        public ActionResult Callback(string code, string state, string error)
        {
            var token = new SpotifyWebApiClient(new BasicAuthenticator()).GetToken(code, RedirectUri);
            
            SpotifyAuthenticator.AddTokenToSession(token);

            return Content(token);
        }
    }
}