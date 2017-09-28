using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineContestSystem.Models;
using OnlineContestSystem.CustomFilters;

namespace OnlineContestSystem.Controllers
{
    [AuthLog(Roles = "Admin")]
    public class KnownTalentsController : Controller
    {
        private readonly ContestantDbContext db = new ContestantDbContext();

        // GET: KnownTalents
        public ActionResult Index()
        {
            return View(db.KnownTalents.ToList());
        }

        // GET: KnownTalents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var knownTalents = db.KnownTalents.Find(id);
            if (knownTalents == null)
                return HttpNotFound();
            return View(knownTalents);
        }

        // GET: KnownTalents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KnownTalents/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,TalentImage")] KnownTalents knownTalents,
            HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var savePath = "~/Images/" + knownTalents.Name + "/";
                    var dir = new DirectoryInfo(HttpContext.Server.MapPath(savePath));
                    if (!dir.Exists)
                        dir.Create();
                    file = Request.Files[i];
                    file.SaveAs(HttpContext.Server.MapPath(savePath)
                                + file.FileName);
                    if (knownTalents.TalentImage != null)
                        knownTalents.TalentImage.Add(
                            new Media {Path = "/Images/" + knownTalents.Name + "/" + file.FileName});
                    else
                        knownTalents.TalentImage =
                            new List<Media> {new Media {Path = "/Images/" + knownTalents.Name + "/" + file.FileName}};
                }

                db.KnownTalents.Add(knownTalents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(knownTalents);
        }

        // GET: KnownTalents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var knownTalents = db.KnownTalents.Find(id);
            if (knownTalents == null)
                return HttpNotFound();
            return View(knownTalents);
        }

        // POST: KnownTalents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,TalentImage")] KnownTalents knownTalents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knownTalents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(knownTalents);
        }

        // GET: KnownTalents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var knownTalents = db.KnownTalents.Find(id);
            if (knownTalents == null)
                return HttpNotFound();
            return View(knownTalents);
        }

        // POST: KnownTalents/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var knownTalents = db.KnownTalents.Find(id);
            db.KnownTalents.Remove(knownTalents);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}