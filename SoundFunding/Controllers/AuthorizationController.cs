using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using RestSharp;
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
            if (token == null || token.HasExpired)
                throw new Exception("Sys!");

            Session["SpotifyToken"] = token;

            new TaskFactory().StartNew(() => GeneratePlaylist(token));


            //foreach (var playlist in playlists.Items)
            //{
            //    var playlistTracks = await Playlist.GetPlaylistTracks(user.Id, playlist.Id, token);
            //    var artists = playlistTracks.Items.Take(5).SelectMany(t => t.Track.Artists).ToList();
            //    foreach (var artist in artists)
            //        allArtists.Add(artist);
            //}

            //var newPlaylistTracks = new ConcurrentBag<Track>();

            //foreach (var artist in allArtists)
            //{
            //    var tracks = Track.GetArtistTopTracks(artist.Id, "SE").Result;
            //    foreach (var track in tracks)
            //        newPlaylistTracks.Add(track);
            //}

            //newPlaylistTracks = new ConcurrentBag<Track>(newPlaylistTracks.DistinctBy(t => t.Id).OrderByDescending(t => t.Popularity).Take(10));

            //var newPlaylist = Playlist.CreatePlaylist(user.Id, "SoundFunding " + DateTime.Today.ToShortDateString(), true, token).Result;
            //newPlaylist.AddTracks(newPlaylistTracks.ToList(), token).RunSynchronously();

            return Content("Foo");
            //return Json(new { playlist = newPlaylist.HREF });
        }

        public ActionResult DataSys()
        {
            return Content(Session["DataSys"] as string);
        }

        public async void GeneratePlaylist(AuthenticationToken token)
        {
            var user = await SpotifyWebAPI.User.GetCurrentUserProfile(token);

            var artists = new List<Artist>();

            var savedTracks = await SpotifyWebAPI.User.GetUsersSavedTracks(token);

            if (savedTracks.Total > 0)
            {
                artists.AddRange(savedTracks.Items.SelectMany(t => t.Artists).ToList());
            }

            if (artists.Count <= 2)
            {
                var playlists = await Playlist.GetUsersPlaylists(user, token);

                var playlistTracks = new List<PlaylistTrack>();

                foreach (var playlist in playlists.Items)
                {
                    
                    //var tracks = await Playlist.GetPlaylistTracks(user.Id, playlist.Id, token);
                    var tracks = GetPlaylistTracks(user.Id, playlist.Id, token);
                    playlistTracks.AddRange(tracks);
                }

                //var playlistTracks = await Playlist.GetPlaylistTracks(user.Id, playlists.Items.First().Id, token);

                artists.AddRange(playlistTracks.SelectMany(t2 => t2.Track.Artists));
            }

            artists = artists.OrderByDescending(a => a.Popularity).Take(5).ToList();
            var artistNames = artists.Select(a => a.Name);

            var client = new RestClient("http://cdn.filtr.com/2.1");
            var request = new RestRequest("/SE/SE/tracks");
            request.AddParameter("artist", artistNames.First());

            var result = client.Execute(request);
            Session["DataSys"] = result.Content;
        }

        private IEnumerable<PlaylistTrack> GetPlaylistTracks(string userId, string playlistId, AuthenticationToken token)
        {
            var client = new RestClient("https://api.spotify.com/v1");
            var request = new RestRequest("/users/{user_id}/playlists/{playlist_id}/tracks");
            request.AddParameter("user_id", userId, ParameterType.UrlSegment);
            request.AddParameter("playlist_id", playlistId, ParameterType.UrlSegment);

            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            
            var result = client.Execute<List<PlaylistTrack>>(request);
            return result.Data;
        }
    }
}