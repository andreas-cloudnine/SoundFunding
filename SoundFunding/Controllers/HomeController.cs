using System.Web.Mvc;

namespace SoundFunding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var db = new SoundFundingDbContext())
            //{
            //    db.Causes.AddOrUpdate(new Cause
            //    {
            //        Name = "Test",
            //        ContributorIds = new List<string>
            //        {
            //            User.Identity.GetUserId()
            //        },
            //        GoalSum = 15353200
            //    });
            //    db.SaveChanges();
            //}

            return View();
        }

        public ActionResult Test()
        {
            return Json(new {hej = "Hipp!"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}