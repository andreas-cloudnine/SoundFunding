using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoundFunding.Startup))]
namespace SoundFunding
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
