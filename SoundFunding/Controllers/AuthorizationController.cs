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

        public ActionResult Callback(string code, string state, string error)
        {
            Authentication.RedirectUri = RedirectUri;

            var token = Authentication.GetAccessToken(code).Result;
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

            var playlists = await Playlist.GetUsersPlaylists(user, token);

            var tasks = playlists.Items.Take(2).Select(p => Playlist.GetPlaylistTracks(user.Id, p.Id, token));

            var playlistTracks = await Task.WhenAll(tasks);

            var artists = playlistTracks.SelectMany(t => t.Items.SelectMany(t2 => t2.Track.Artists));
            var artistNames = artists.Select(a => a.Name);

            var client = new RestClient("http://cdn.filtr.com/2.1");
            var request = new RestRequest("/SE/SE/tracks?artist={artist}");
            request.AddParameter("artist", artistNames.First());

            var result = client.Execute(request);
            Session["DataSys"] = result.Content;
        }
    }
}