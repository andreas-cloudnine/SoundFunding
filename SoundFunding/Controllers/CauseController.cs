using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SoundFunding.Classes;
using SoundFunding.Models;
using SpotifyWebAPI;

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
                        Value = "0",
                        Text = "Set your goal!"
                    },
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
        public async Task<ActionResult> Create(Cause cause)
        {
            if (ModelState.IsValid)
            {
                if(cause.PostedPicture != null)
                { 
                    var bh = new BlobHandler("soundfunding");
                    bh.Upload(new List<HttpPostedFileBase> {cause.PostedPicture});
                    var blobUri = bh.GetBlobs().FirstOrDefault(b => b.Contains(cause.PostedPicture.FileName));
                    cause.Picture = blobUri;
                }

                var token = Session["SpotifyToken"] as AuthenticationToken;
                var playlist = await PlaylistGenerator.GeneratePlaylist(token);
                
                cause.SpotifyPlaylistUri = playlist.Uri;
                cause.SpotifyUserAvatarUrl = playlist.Owner.Images.First().Url;

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
                var cause = db.Causes.Find(id);

                cause.OtherCauses = db.Causes.ToList();

                return View(cause);
            }
        }
    }
}