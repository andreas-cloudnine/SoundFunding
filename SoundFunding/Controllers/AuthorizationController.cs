using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using RestSharp;
using SoundFunding.Classes;
using SpotifyWebAPI;
using SpotifyWebAPI.SpotifyModel;

namespace SoundFunding.Controllers
{
    public class AuthorizationController : Controller
    {
        private const string RedirectUri = "http://soundfunding.azurewebsites.net/authorization/callback/";

        public ActionResult Login(string redirectUri = null)
        {
            var url = "https://accounts.spotify.com/authorize?client_id={client_id}&response_type={response_type}&redirect_uri={redirect_uri}&scope={scope}";

            Session["LoginRedirectUrl"] = redirectUri;

            url = url.Replace("{client_id}", Config.ClientID);
            url = url.Replace("{response_type}", "code");
            url = url.Replace("{redirect_uri}", Url.Encode(RedirectUri));
            url = url.Replace("{scope}", string.Join(" ", Config.AuthorizationScopes));

            return Redirect(url);
        }

        public async Task<ActionResult> Callback(string code, string state, string error)
        {
            Authentication.RedirectUri = RedirectUri;

            var token = await Authentication.GetAccessToken(code);
            if (token == null || token.HasExpired)
                throw new Exception("Sys!");

            Session["SpotifyToken"] = token;

            new TaskFactory().StartNew(() => PlaylistGenerator.GenerateAndStorePlaylistTracks(token));

            Thread.Sleep(TimeSpan.FromSeconds(10));

            var redirect = Session["LoginRedirectUrl"] as string;
            if (redirect != null)
            {
                return Redirect(redirect);
            }

            return Redirect(Url.RouteUrl("default", new {controller = "Cause", action = "Create"}));
        }

        //private IEnumerable<PlaylistTrack> GetPlaylistTracks(string userId, string playlistId, AuthenticationToken token)
        //{
        //    var client = new RestClient("https://api.spotify.com/v1");
        //    var request = new RestRequest("/users/{user_id}/playlists/{playlist_id}/tracks");
        //    request.RequestFormat = DataFormat.Json;

        //    request.AddParameter("user_id", userId, ParameterType.UrlSegment);
        //    request.AddParameter("playlist_id", playlistId, ParameterType.UrlSegment);

        //    request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            
        //    var result = client.Execute<List<PlaylistTrack>>(request);
        //    return result.Data;
        //}
    }
}