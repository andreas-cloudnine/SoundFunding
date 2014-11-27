using System.Web.Http;
using SoundFunding.SpotifyClient;

namespace SoundFunding.Controllers.WebApi
{
    public class AuthorizationApiController : ApiController
    {
        private const string RedirectUri = "/api/authorizationapi/callback";

        [HttpGet]
        public IHttpActionResult Login()
        {
            new SpotifyWebApiClient().Authorize(RedirectUri);
            return Ok();
        }

        public string Callback(string code, string state)
        {
            var token = new SpotifyWebApiClient(new BasicAuthenticator()).GetToken(code, RedirectUri);
            SpotifyAuthenticator.AddTokenToSession(token);
            return token;
        }
    }
}
