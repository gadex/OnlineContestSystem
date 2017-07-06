﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineContestSystem.Models;

namespace OnlineContestSystem.Controllers
{
    public class ContestantsController : Controller
    {
        private ContestantDbContext db = new ContestantDbContext();

        // GET: Contestants
        public ActionResult Index()
        {
            return View(db.Contestants.OrderByDescending(x => x.ID).Take(10).ToList());
        }

        // GET: Contestants/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Find(id);
            if (contestant == null)
            {
                return HttpNotFound();
            }
            return View(contestant);
        }

        [Authorize]
        public ActionResult Categories(int? id, string catFinal)
        {
            var categoryList = new List<Category>();
            var categoryQry = from d in db.Contestants
                orderby d.Categories
                select d.Categories;

            var categList = from c in db.Contestants
                select c;

            categoryList.AddRange(categoryQry.Distinct());
            ViewBag.catFinal = new SelectList(categoryList);

            if (!string.IsNullOrEmpty(catFinal))
            {
                categList = categList.Where(x => x.Categories.ToString() == catFinal);
            }

            catFinal = categList.ToString();

            return View(catFinal);
        }

        // GET: Contestants/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contestants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,PhoneNumber,DateOfBirth,Nation,States,Categories")] Contestant contestant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string savePath = "~/Images/" + @User.Identity.GetUserId() + "/" + @contestant.Name + "/";
                    DirectoryInfo dir = new DirectoryInfo(HttpContext.Server.MapPath(savePath));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    file = Request.Files[i];
                    file.SaveAs(HttpContext.Server.MapPath(savePath)
                                + file.FileName);
                    if (contestant.Images != null) contestant.Images.Add(new Models.Media { Path = "/Images/" + ((string.IsNullOrEmpty(@User.Identity.GetUserId())) ? "" : @User.Identity.GetUserId() + "/") + @contestant.Name + "/" + file.FileName });
                    else contestant.Images = new List<Models.Media> { new Models.Media { Path = "/Images/" + ((string.IsNullOrEmpty(@User.Identity.GetUserId())) ? "" : @User.Identity.GetUserId() + "/") + @contestant.Name + "/" + file.FileName } };
                    


                }

                db.Contestants.Add(contestant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contestant);
        }

        // GET: Contestants/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Find(id);
            if (contestant == null)
            {
                return HttpNotFound();
            }
            return View(contestant);
        }

        // POST: Contestants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,PhoneNumber,DateOfBirth,Nation,States")] Contestant contestant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contestant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contestant);
        }

        // GET: Contestants/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Find(id);
            if (contestant == null)
            {
                return HttpNotFound();
            }
            return View(contestant);
        }

        // POST: Contestants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contestant contestant = db.Contestants.Find(id);
            db.Contestants.Remove(contestant);
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