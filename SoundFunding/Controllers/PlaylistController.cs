using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SoundFunding.Classes;
using SpotifyWebAPI;

namespace SoundFunding.Controllers
{
    public class PlaylistController : Controller
    {
        // GET: Playlist
        public async Task<ActionResult> Generate()
        {
            var token = Session[Config.TokenSessionKey] as AuthenticationToken;

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