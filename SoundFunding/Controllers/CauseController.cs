using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
            var token = Session["SpotifyToken"] as AuthenticationToken;
            if (token == null)
                throw new Exception("Not logged in against Spotify");

            if (ModelState.IsValid)
            {
                if(cause.PostedPicture != null)
                { 
                    var bh = new BlobHandler("soundfunding");
                    bh.Upload(new List<HttpPostedFileBase> {cause.PostedPicture});
                    var blobUri = bh.GetBlobs().FirstOrDefault(b => b.Contains(cause.PostedPicture.FileName));
                    cause.Picture = blobUri;
                }

                var playlist = await PlaylistGenerator.GeneratePlaylist(token, cause);

                cause.SpotifyPlaylistId = playlist.Id;
                cause.SpotifyPlaylistUri = playlist.Uri;
                cause.SpotifyUserId = playlist.Owner.Id;

                var image = playlist.Owner.Images.FirstOrDefault();
                if (image != null)
                    cause.SpotifyUserAvatarUrl = image.Url;

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

                return View(cause);
            }
        }

        public async Task<ActionResult> Join(int id)
        {
            var token = Session["SpotifyToken"] as AuthenticationToken;
            if (token == null)
                throw new Exception("Not logged in against Spotify");
            using (var db = new SoundFundingDbContext())
            {
                var cause = db.Causes.Find(id);

                if (cause.SpotifyPlaylistId != null)
                {
                    var user = await PlaylistGenerator.AddToPlaylist(token, cause);
                    var image = user.Images.FirstOrDefault();
                    if (image != null)
                    {
                        cause.Contributors = cause.Contributors ?? new List<Contributor>();
                        cause.Contributors.Add(new Contributor
                        {
                            ImageUrl = image.Url
                        });
                    }
                }

                db.SaveChanges();
            }
            return RedirectToAction("Cause", new {id = id});
        }
    }
}