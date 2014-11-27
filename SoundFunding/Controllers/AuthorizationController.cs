using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SoundFunding.Classes;
using SpotifyWebAPI;

namespace SoundFunding.Controllers
{
    public class AuthorizationController : Controller
    {
        private const string RedirectUri = "http://soundfunding.azurewebsites.net/authorization/callback/";

        public ActionResult Login()
        {
            var url = "https://accounts.spotify.com/authorize?client_id={client_id}&response_type={response_type}&redirect_uri={redirect_uri}&scope={scope}";

            url = url.Replace("{client_id}", Config.ClientID);
            url = url.Replace("{response_type}", "code");
            url = url.Replace("{redirect_uri}", Url.Encode(RedirectUri));
            url = url.Replace("{scope}", string.Join(" ", Config.AuthorizationScopes));

            return Redirect(url);
        }

        public ActionResult Callback(string code, string state, string error)
        {
            Authentication.RedirectUri = RedirectUri;

            var token = Authentication.GetAccessToken(code).Result;
            if (token == null || token.HasExpired)
                throw new Exception("Sys!");

            Session["SpotifyToken"] = token;

            var user = SpotifyWebAPI.User.GetCurrentUserProfile(token).Result;

            var playlists = Playlist.GetUsersPlaylists(user, token).Result;

            var allArtists = new ConcurrentBag<Artist>();

            foreach (var playlist in playlists.Items)
            {
                var playlistTracks = Playlist.GetPlaylistTracks(user.Id, playlist.Id, token).Result;
                var artists = playlistTracks.Items.Take(2).SelectMany(t => t.Track.Artists).ToList();
                foreach (var artist in artists)
                    allArtists.Add(artist);
            }

            allArtists = new ConcurrentBag<Artist>(allArtists.ToArray().DistinctBy(a => a.Id));

            var newPlaylistTracks = new ConcurrentBag<Track>();

            foreach (var artist in allArtists)
            {
                var tracks = Track.GetArtistTopTracks(artist.Id, "SE").Result;
                foreach (var track in tracks)
                    newPlaylistTracks.Add(track);
            }

            newPlaylistTracks = new ConcurrentBag<Track>(newPlaylistTracks.DistinctBy(t => t.Id).OrderByDescending(t => t.Popularity).Take(10));

            var newPlaylist = Playlist.CreatePlaylist(user.Id, "SoundFunding " + DateTime.Today.ToShortDateString(), true, token).Result;
            newPlaylist.AddTracks(newPlaylistTracks.ToList(), token).RunSynchronously();

            return Json(new { playlist = newPlaylist.HREF });
        }
    }
}