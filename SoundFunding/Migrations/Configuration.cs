using System.Collections.Generic;
using SoundFunding.Models;

namespace SoundFunding.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SoundFundingDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SoundFunding.Models.SoundFundingDbContext";
        }

        protected override void Seed(SoundFundingDbContext db)
        {
            db.Causes.Add(new Cause
            {
                Name = "R�dda tigern",
                ReceivingOrganization = "WWF",
                GoalSum = 500,
                Picture = "/html/img/bg-tiger-400f.png",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg",
                Contributors = new List<Contributor>()
                {
                    new Contributor
                    {
                        ImageUrl = "/html/img/andreas.jpg"
                    },
                    new Contributor
                    {
                        ImageUrl = "/html/img/andreas.jpg"
                    },
                    new Contributor
                    {
                        ImageUrl = "/html/img/andreas.jpg"
                    },
                }
            });
 
            db.Causes.Add(new Cause
            {
                Name = "K�p en flock getter � Actionaid",
                ReceivingOrganization = "Actionaid",
                GoalSum = 500,
                Picture = "/html/img/get2.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg" 
            });
            db.Causes.Add(new Cause
            {
                Name = "Kvinnors r�ttigheter- Amnesty international",
                ReceivingOrganization = "Amnesty international",
                GoalSum = 500,
                Picture = "/html/img/kvinnorsr�tt2.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg" 
            });
            db.Causes.Add(new Cause
            {
                Name = "St�d jourtelefonen f�r barn - Bris",
                ReceivingOrganization = "Bris",
                GoalSum = 500,
                Picture = "/html/img/bris.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"
            });
            db.Causes.Add(new Cause
            {
                Name = "Kampen mot ebola - l�kare utan gr�nser",
                ReceivingOrganization = "L�kare utan gr�nser",
                GoalSum = 500,
                Picture = "/html/img/ebola2.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"  
            });
            db.Causes.Add(new Cause
            {
                Name = "Hj�lp Stockholms heml�sa - Stadsmissionen",
                ReceivingOrganization = "Stadsmissionen",
                GoalSum = 500,
                Picture = "/html/img/heml�s.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"   
            });
            db.Causes.Add(new Cause 
            {
                Name = "Stoppa vapenhandeln � Amnesty International",
                ReceivingOrganization = "Amnesty International",
                GoalSum = 500,
                Picture = "/html/img/noguns.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg" 
            });
            db.Causes.Add(new Cause
            {
                Name = "Plantera tr�d - Vi-skogen",
                ReceivingOrganization = "WWF",
                GoalSum = 500,
                Picture = "/html/img/tree.jpg"   ,
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"
            });
            db.Causes.Add(new Cause
            {
                Name = "Stoppa valfisket - WWF",
                ReceivingOrganization = "WWF",
                GoalSum = 500,
                Picture = "/html/img/whale.jpg",
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"
            }); 
            db.Causes.Add(new Cause
            {
                Name = "Vaccinering av barn -unicef",
                ReceivingOrganization = "Unicef",
                GoalSum = 500,
                Picture = "/html/img/vaccin2.jpg",                 
                SpotifyPlaylistUri = "spotify:user:sonymusicentertainment:playlist:2BkvZh4A1UVPQIps6ILKvd",
                SpotifyUserAvatarUrl = "/html/img/andreas.jpg"
            });

            db.SaveChanges();

        }
    }
}
