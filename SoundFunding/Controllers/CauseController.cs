using System;
using System.Web.Mvc;
using SoundFunding.Models;

namespace SoundFunding.Controllers
{
    public class CauseController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cause cause)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SoundFundingDbContext())
                {
                    db.Causes.Add(cause);
                }
                return RedirectToAction("Index","Home");
            }
            return View("Create");
        }
    }
}