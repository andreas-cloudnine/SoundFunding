using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SoundFunding.Migrations;
using SpotifyWebAPI;

namespace SoundFunding.Models
{
    public class SoundFundingDbContext : IdentityDbContext<ApplicationUser>
    {
        public SoundFundingDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SoundFundingDbContext, Configuration>());
        }

        public static SoundFundingDbContext Create()
        {
            return new SoundFundingDbContext();
        }

        public DbSet<Cause> Causes { get; set; }
        public DbSet<PlaylistTracks> Tracks { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
    }
}