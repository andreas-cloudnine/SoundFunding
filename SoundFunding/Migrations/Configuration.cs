using SoundFunding.Models;

namespace SoundFunding.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SoundFunding.Models.SoundFundingDbContext>
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
                Name = "Rädda tigern",
                ReceivingOrganization = "WWF",
                GoalSum = 500,
                Picture = "/html/img/bg-tiger-400f.png"
            });
        }
    }
}
