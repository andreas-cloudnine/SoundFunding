using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SoundFunding.Models
{
    public class SoundFundingDbContext : IdentityDbContext<ApplicationUser>
    {
        public SoundFundingDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static SoundFundingDbContext Create()
        {
            return new SoundFundingDbContext();
        }

        public DbSet<Cause> Causes { get; set; }
    }
}