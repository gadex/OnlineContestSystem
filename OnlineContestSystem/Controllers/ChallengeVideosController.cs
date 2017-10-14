using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineContestSystem.CustomFilters;
using OnlineContestSystem.Models;

namespace OnlineContestSystem.Controllers
{
    [AuthLog(Roles = "Admin")]
    public class ChallengeVideosController : Controller
    {
        private ContestantDbContext db = new ContestantDbContext();

        // GET: ChallengeVideos
        public ActionResult Index()
        {
            return View(db.ChallengeVideos.ToList());
        }

        // GET: ChallengeVideos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChallengeVideo challengeVideo = db.ChallengeVideos.Find(id);
            if (challengeVideo == null)
            {
                return HttpNotFound();
            }
            return View(challengeVideo);
        }

        // GET: ChallengeVideos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChallengeVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UploadDate,ChallengeVid")] ChallengeVideo challengeVideo, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string savePath = "~/Images/ChallengeVideos" + challengeVideo.Id + "/";
                    DirectoryInfo dir = new DirectoryInfo(HttpContext.Server.MapPath(savePath));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    file = Request.Files[i];
                    file.SaveAs(HttpContext.Server.MapPath(savePath)
                                + file.FileName);
                    if (challengeVideo.ChallengeVid != null)
                        challengeVideo.ChallengeVid.Add(new Models.Media
                        {
                            Path = "/Images/ChallengeVideos" + challengeVideo.Id + "/" + file.FileName
                        });
                    else
                        challengeVideo.ChallengeVid = new List<Models.Media>
                        {
                            new Models.Media
                            {
                                Path = "/Images/ChallengeVideos" + challengeVideo.Id + "/" + file.FileName
                            }
                        };



                }
                challengeVideo.UploadDate = DateTime.Now;
                db.ChallengeVideos.Add(challengeVideo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(challengeVideo);
        }

        // GET: ChallengeVideos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChallengeVideo challengeVideo = db.ChallengeVideos.Find(id);
            if (challengeVideo == null)
            {
                return HttpNotFound();
            }
            return View(challengeVideo);
        }

        // POST: ChallengeVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UploadDate")] ChallengeVideo challengeVideo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(challengeVideo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(challengeVideo);
        }

        // GET: ChallengeVideos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChallengeVideo challengeVideo = db.ChallengeVideos.Find(id);
            if (challengeVideo == null)
            {
                return HttpNotFound();
            }
            return View(challengeVideo);
        }

        // POST: ChallengeVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChallengeVideo challengeVideo = db.ChallengeVideos.Find(id);
            db.ChallengeVideos.Remove(challengeVideo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
