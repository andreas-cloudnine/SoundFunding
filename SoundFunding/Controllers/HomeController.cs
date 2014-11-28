using System.Linq;
using System.Web.Mvc;
using SoundFunding.Models;

namespace SoundFunding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new SoundFundingDbContext())
            {
                ViewBag.OtherCauses = db.Causes.ToList();

                return View();
            }
        }
    }
}