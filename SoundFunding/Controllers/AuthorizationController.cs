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
using SpotifyWebAPI.SpotifyModel;

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

                var playlist = playlists.Items.First();
                
                var tracks = await Playlist.GetPlaylistTracks(user.Id, playlist.Id, token);
                //var tracks = GetPlaylistTracks(user.Id, playlist.Id, token);
                playlistTracks.AddRange(tracks.Items);

                //var playlistTracks = await Playlist.GetPlaylistTracks(user.Id, playlists.Items.First().Id, token);

                artists.AddRange(playlistTracks.SelectMany(t2 => t2.Track.Artists));
            }

            artists = artists.DistinctBy(a => a.Name).OrderByDescending(a => a.Popularity).Take(5).ToList();
            var artistNames = artists.Select(a => a.Name);

            var newPlaylistTrackIds = new List<string>();
            
            foreach (var artistName in artistNames)
            {
                var client = new RestClient("http://cdn.filtr.com/2.1");
                var request = new RestRequest("/SE/SE/tracks");
                request.AddParameter("artist", artistName);

                var result = client.Execute<List<FiltrTrack>>(request);
                newPlaylistTrackIds.AddRange(
                    result.Data.OrderByDescending(t => t.hotness)
                        .Take(5)
                        .Select(t => t.spotifyUri.Replace("spotify:track:", string.Empty)));
            }

            var newPlaylistTracks = await Track.GetTracks(newPlaylistTrackIds);
            var newPlaylist = Playlist.CreatePlaylist(user.Id, "SoundFunding " + DateTime.Today.ToShortDateString(), true, token).Result;
            await newPlaylist.AddTracks(newPlaylistTracks.OrderBy(t => t.Popularity).Take(5).ToList(), token);
        }

        private IEnumerable<PlaylistTrack> GetPlaylistTracks(string userId, string playlistId, AuthenticationToken token)
        {
            var client = new RestClient("https://api.spotify.com/v1");
            var request = new RestRequest("/users/{user_id}/playlists/{playlist_id}/tracks");
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("user_id", userId, ParameterType.UrlSegment);
            request.AddParameter("playlist_id", playlistId, ParameterType.UrlSegment);

            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            
            var result = client.Execute<List<PlaylistTrack>>(request);
            return result.Data;
        }
    }

    public class FiltrTrack
    {
        public string spotifyUri { get; set; }
        public double? hotness { get; set; }
    }
    //internal class playlisttrack
    //{
    //    public string added_at { get; set; }

    //    public user added_by { get; set; }

    //    public track track { get; set; }

    //    public PlaylistTrack ToPOCO()
    //    {
    //        DateTime result;
    //        return new PlaylistTrack()
    //        {
    //            AddedAt = !DateTime.TryParse(this.added_at, out result) ? DateTime.Now : result,
    //            AddedBy = this.added_by.ToPOCO(),
    //            Track = this.track.ToPOCO()
    //        };
    //    }
    //}
    //internal class user
    //{
    //    public string id { get; set; }
    //}
    //internal class track
    //{
    //    public album album { get; set; }

    //    public artist[] artists { get; set; }

    //    public string[] available_markets { get; set; }

    //    public int disc_number { get; set; }

    //    public int duration_ms { get; set; }

    //    public bool @explicit { get; set; }

    //    public external_ids external_ids { get; set; }

    //    public external_urls external_urls { get; set; }

    //    public string href { get; set; }

    //    public string id { get; set; }

    //    public string name { get; set; }

    //    public int popularity { get; set; }

    //    public string preview_url { get; set; }

    //    public int track_number { get; set; }

    //    public string type { get; set; }

    //    public string uri { get; set; }

    //    public Track ToPOCO()
    //    {
    //        Track track = new Track();
    //        if (this.album != null)
    //            track.Album = this.album.ToPOCO();
    //        if (this.artists != null)
    //        {
    //            foreach (artist artist in this.artists)
    //                track.Artists.Add(artist.ToPOCO());
    //        }
    //        if (this.available_markets != null)
    //            track.AvailableMarkets = Enumerable.ToList<string>((IEnumerable<string>)this.available_markets);
    //        track.DiscNumber = this.disc_number;
    //        track.Duration = this.duration_ms;
    //        if (this.external_ids != null)
    //            track.ExternalId = this.external_ids.ToPOCO();
    //        if (this.external_urls != null)
    //            track.ExternalUrl = this.external_urls.ToPOCO();
    //        if (this.href != null)
    //            track.HREF = this.href;
    //        if (this.id != null)
    //            track.Id = this.id;
    //        if (this.name != null)
    //            track.Name = this.name;
    //        track.Popularity = this.popularity;
    //        if (this.preview_url != null)
    //            track.PreviewUrl = this.preview_url;
    //        track.TrackNumber = this.track_number;
    //        if (this.type != null)
    //            track.Type = this.type;
    //        if (this.uri != null)
    //            track.Uri = this.uri;
    //        return track;
    //    }
    //}

}