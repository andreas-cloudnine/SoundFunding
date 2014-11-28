using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            // Try getting artists from saved tracks
            if (savedTracks.Total > 0)
            {
                artists.AddRange(savedTracks.Items.SelectMany(t => t.Artists).ToList());
            }

            // Try get by most popular user playlist
            if (artists.Count < 2)
            {
                var playlists = await Playlist.GetUsersPlaylists(user, token);

                var playlistTracks = new List<PlaylistTrack>();

                var playlist = playlists.Items.FirstOrDefault();

                if (playlist != null)
                {
                    var tracks = await Playlist.GetPlaylistTracks(user.Id, playlist.Id, token);

                    playlistTracks.AddRange(tracks.Items);

                    artists.AddRange(playlistTracks.SelectMany(t2 => t2.Track.Artists));
                }
            }

            if (artists.Count < 2)
            {
                return; //Give up
            }

            artists = artists.DistinctBy(a => a.Name).OrderByDescending(a => a.Popularity).Take(5).ToList();
            var artistNames = artists.Select(a => a.Name).ToList();

            var tracksPerArtist = 10/artistNames.Count();

            var newPlaylistTrackIds = new List<string>();

            foreach (var artistName in artistNames)
            {
                var client = new RestClient("http://cdn.filtr.com/2.1");
                var request = new RestRequest("/SE/SE/tracks");
                request.AddParameter("artist", artistName);

                var result = client.Execute<List<FiltrTrack>>(request);
                newPlaylistTrackIds.AddRange(
                    result.Data.OrderByDescending(t => t.hotness)
                        .Take(tracksPerArtist)
                        .Select(t => t.spotifyUri.Replace("spotify:track:", string.Empty)));
            }

            using (var db = new SoundFundingDbContext())
            {
                db.Tracks.Add(new PlaylistTracks { UserId = user.Id, TrackIds = string.Join(",", newPlaylistTrackIds.Distinct()) });
                db.SaveChanges();
            }
        }

        public static async Task<Playlist> GeneratePlaylist(AuthenticationToken token, Cause cause, int attempt = 1)
        {
            var user = await User.GetCurrentUserProfile(token);
            
            using (var db = new SoundFundingDbContext())
            {
                var stored = db.Tracks.OrderByDescending(t => t.Id).FirstOrDefault(t => t.UserId == user.Id );
                if (stored == null)
                {
                    if (attempt <= 3)
                    {
                        // Might not be generated yet
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                        return await GeneratePlaylist(token, cause, ++attempt);
                    }
                    else
                    {
                        var filrPlaylist = GetStandardFiltrPlaylist();
                        return new Playlist
                        {
                            Owner = user,
                            Uri = filrPlaylist.spotifyUri
                        };
                    }
                }

                var newPlaylistTracks = await Track.GetTracks(stored.TrackIds.Split(',').ToList());
                var newPlaylist = await Playlist.CreatePlaylist(user.Id, "SoundFunding " + cause.Name, true, token);
                await newPlaylist.AddTracks(newPlaylistTracks.OrderBy(t => t.Popularity).Take(5).ToList(), token);
                return newPlaylist;
            }
        }

        public static FiltrPlaylist GetStandardFiltrPlaylist()
        {
            var client = new RestClient("http://cdn.filtr.com/2.1");
            var request = new RestRequest("/SE/SE/playlists");

            var result = client.Execute<List<FiltrPlaylist>>(request);

            return result.Data.OrderByDescending(p => p.hotness).First();
        }

        public static async void AddToPlaylist(AuthenticationToken token, Cause cause)
        {
            var user = await User.GetCurrentUserProfile(token);

            using (var db = new SoundFundingDbContext())
            {
                var stored = db.Tracks.LastOrDefault(t => t.UserId == user.Id);
                if (stored == null)
                {
                }

                var newPlaylistTracks = await Track.GetTracks(stored.TrackIds.Split(',').ToList());
                var playlist = await Playlist.GetPlaylist(cause.SpotifyUserId, cause.SpotifyPlaylistId, token);
                await playlist.AddTracks(newPlaylistTracks.OrderBy(t => t.Popularity).Take(5).ToList(), token);
            }
        }

    }

    public class FiltrTrack
    {
        public string spotifyUri { get; set; }
        public double? hotness { get; set; }
    }
    public class FiltrPlaylist
    {
        public string spotifyUri { get; set; }
        public double? hotness { get; set; }
    }
}