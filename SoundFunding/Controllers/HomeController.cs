using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SoundFunding.Models;

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