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
                })
            };
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cause cause)
        {
            if (ModelState.IsValid)
            {
                BlobHandler bh = new BlobHandler("containername");
                bh.Upload(new List<HttpPostedFileBase> {cause.PostedPicture});
                var blobUri = bh.GetBlobs().FirstOrDefault();

                cause.Picture = blobUri;

                if(cause.ContributorIds == null)
                    cause.ContributorIds = new List<string>();

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