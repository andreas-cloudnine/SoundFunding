using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoundFunding.Models;

namespace SoundFunding.Controllers
{
    public class CauseController : Controller
    {
        public ActionResult Index(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var cause = new Cause
            {
                GoalSums = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "50",
                        Text = "$50"
                    },
                    new SelectListItem
                    {
                        Value = "75",
                        Text = "$75"
                    },
                    new SelectListItem
                    {
                        Value = "100",
                        Text = "$100"
                    },
                    new SelectListItem
                    {
                        Value = "150",
                        Text = "$150"
                    },
                    new SelectListItem
                    {
                        Value = "200",
                        Text = "$200"
                    },
                    new SelectListItem
                    {
                        Value = "500",
                        Text = "$500"
                    }
                }, "Value", "Text")
            };
            return View(cause);
        }

        [HttpPost]
        public ActionResult Create(Cause cause)
        {
            if (ModelState.IsValid)
            {
                BlobHandler bh = new BlobHandler("soundfunding");
                bh.Upload(new List<HttpPostedFileBase> {cause.PostedPicture});
                var blobUri = bh.GetBlobs().FirstOrDefault();

                cause.Picture = blobUri;

                using (var db = new SoundFundingDbContext())
                {
                    db.Causes.Add(cause);
                    db.SaveChanges();
                }
                return RedirectToAction("Cause", "Cause", new { id = cause.Id });
            }
            return View("Create");
        }

        public ActionResult Cause(int id)
        {
            using (var db = new SoundFundingDbContext())
            {
                return View(db.Causes.Find(id));
            }
        }
    }
}