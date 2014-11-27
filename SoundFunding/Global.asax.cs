using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SoundFunding.Classes;

namespace SoundFunding
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            SpotifyWebAPI.Authentication.ClientId = Config.ClientID;
            SpotifyWebAPI.Authentication.ClientSecret = Config.ClientSecret;
        }

    }
}
