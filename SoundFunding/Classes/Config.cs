using System.Collections.Generic;

namespace SoundFunding.Classes
{
    public class Config
    {
        internal const string ClientID = "28c9a4868be14e358a3256919946c7fd";
        internal const string ClientSecret = "2d687b1ebeba4fcbbdfde3031ab4ec01";

        internal static readonly IEnumerable<string> AuthorizationScopes = new[] {
                                                                                    "playlist-modify-public",
                                                                                    "user-read-private"
                                                                                };

        internal const string TokenSessionKey = "SpotifyKey";
    }
}