using System;
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

        public async Task<ActionResult> Callback(string code, string state, string error)
        {
            Authentication.RedirectUri = RedirectUri;

            var token = await Authentication.GetAccessToken(code);

            Session["SpotifyToken"] = token;

            var user = await SpotifyWebAPI.User.GetCurrentUserProfile(token);

            var playlists = await Playlist.GetUsersPlaylists(user, token);

            var allArtists = new List<Artist>();

            foreach (var playlist in playlists.Items)
            {
                var playlistTracks = await Playlist.GetPlaylistTracks(user.Id, playlist.Id, token);
                var artists = playlistTracks.Items.SelectMany(t => t.Track.Artists);
                allArtists.AddRange(artists);
            }

            allArtists = allArtists.DistinctBy(a => a.Id).ToList();

            var newPlaylistTracks = new List<Track>();

            foreach (var artist in allArtists)
            {
                var tracks = await Track.GetArtistTopTracks(artist.Id, "SE");
                newPlaylistTracks.AddRange(tracks);
            }

            newPlaylistTracks = newPlaylistTracks.DistinctBy(t => t.Id).OrderByDescending(t => t.Popularity).Take(10).ToList();

            var newPlaylist = await Playlist.CreatePlaylist(user.Id, "SoundFunding " + DateTime.Today.ToShortDateString(), true, token);
            await newPlaylist.AddTracks(newPlaylistTracks, token);

            return Json(new { playlist = newPlaylist.HREF });
        }
    }
}