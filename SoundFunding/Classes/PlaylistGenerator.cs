using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using RestSharp;
using SoundFunding.Models;
using SpotifyWebAPI;

namespace SoundFunding.Classes
{
    public static class PlaylistGenerator
    {
        public static async void GenerateAndStorePlaylistTracks(AuthenticationToken token)
        {
            var user = await User.GetCurrentUserProfile(token);

            var artists = new List<Artist>();

            var savedTracks = await User.GetUsersSavedTracks(token);

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
                        .Take(2)
                        .Select(t => t.spotifyUri.Replace("spotify:track:", string.Empty)));
            }

            using (var db = new SoundFundingDbContext())
            {
                db.Tracks.Add(new PlaylistTracks { UserId = user.Id, TrackIds = string.Join(",", newPlaylistTrackIds.Distinct()) });
                db.SaveChanges();
            }
        }

        public static async Task<Playlist> GeneratePlaylist(AuthenticationToken token)
        {
            var user = await User.GetCurrentUserProfile(token);
            
            using (var db = new SoundFundingDbContext())
            {
                var stored = db.Tracks.LastOrDefault(t => t.UserId == user.Id );
                if (stored == null)
                {
                    return null; //TODO: Fetch Filtr standard playlist
                }

                var newPlaylistTracks = await Track.GetTracks(stored.TrackIds.Split(',').ToList());
                var newPlaylist = Playlist.CreatePlaylist(user.Id, "SoundFunding " + DateTime.Today.ToShortDateString(), true, token).Result;
                await newPlaylist.AddTracks(newPlaylistTracks.OrderBy(t => t.Popularity).Take(5).ToList(), token);
                return newPlaylist;
            }
        }
    }

    public class FiltrTrack
    {
        public string spotifyUri { get; set; }
        public double? hotness { get; set; }
    }
}