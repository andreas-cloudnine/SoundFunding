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