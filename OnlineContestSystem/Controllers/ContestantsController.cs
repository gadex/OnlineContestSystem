using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineContestSystem.Models;
using OnlineContestSystem.ViewModels;
using PagedList;

namespace OnlineContestSystem.Controllers
{
    [HandleError]
    public class ContestantsController : Controller
    {
        private readonly ContestantDbContext db = new ContestantDbContext();

        // GET: Contestants
        [HandleError]
        public ActionResult Index(int? Id, int? page)
        {

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel()
            {
                Contestants = db.Contestants.OrderByDescending(x => x.ID).Take(10).ToList(),
                Categories = db.Categories.ToList(),
                KnownTalents = db.KnownTalents.ToList(),
                Blogs = db.Blogs.ToList()
            };
            var count = db.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = db.Messages
                .OrderByDescending(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = db.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = db.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }
            if (Request.IsAjaxRequest())
            {
                //viewModel=your viewModel
                return PartialView("_ChallengeSection", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult Candidates()
        {
            return View(db.Contestants.ToList());
        }

        // GET: Contestants/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contestant = db.Contestants.Find(id);
            if (contestant == null)
                return HttpNotFound();
            return View(contestant);
        }

        public ActionResult Post(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var blog = db.Blogs.Find(id);
            if (blog == null)
                return HttpNotFound();
            return View(blog);
        }


        public async Task<ActionResult> Categories(int? id, string searchString, string msg = "")
        {
            var contestants = from m in db.Contestants
                select m;

            var category = await db.Categories.FindAsync(id);
            if (category == null) return RedirectToAction("Index");

            var c = new VoteCategoryViewModel
            {
                Category = category,
                Contestants = db.Contestants.Where
                    (a => a.Category.Id == category.Id).ToList()
            };

            if (!string.IsNullOrEmpty(searchString))
                c = new VoteCategoryViewModel
                {
                    Category = category,
                    Contestants = db.Contestants.Where(
                        x => x.Name.ToUpper().Contains(searchString.ToUpper()) &&
                             x.Category.Id == category.Id).ToList()
                };


            return View(c);
        }

        public ActionResult AllCategories(int? id, string contCategory, string searchString, string sortOrder,
            string msg = "")
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CatSortParm = sortOrder == "Categoty" ? "cat_desc" : "Category";

            var conts = from g in db.Contestants
                select g;

            var catLst = new List<string>();

            var catQuery = from d in db.Contestants
                orderby d.Category.Title
                select d.Category.Title;

            catLst.AddRange(catQuery.Distinct());
            ViewBag.contCategory = new SelectList(catLst, 0);

            var contestants = from m in db.Contestants
                select m;
            var c = new VoteCategoryViewModel
            {
                Contestants = db.Contestants.ToList()
            };

            if (!string.IsNullOrEmpty(searchString))
                c = new VoteCategoryViewModel
                {
                    Contestants = db.Contestants.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper())).ToList()
                };

            if (!string.IsNullOrEmpty(contCategory))
                c = new VoteCategoryViewModel
                {
                    Contestants = db.Contestants.Where(x => x.Category.Title == contCategory).ToList()
                };

            if (!string.IsNullOrEmpty(searchString) && !string.IsNullOrEmpty(contCategory))
                c = new VoteCategoryViewModel
                {
                    Contestants = db.Contestants
                        .Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) &&
                                    x.Category.Title == contCategory).ToList()
                };

            return View(c);
        }

        // GET: Contestants/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Title");
            return View();
        }

        [Authorize]
        public ActionResult Vote(int contestantId, int categoryId)
        {
            var uid = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(uid)) return RedirectToAction("Login", "Account");
            if (db.Voters.Any(a => a.UserId == uid && a.CategoryId == categoryId))

                return RedirectToAction("Categories",
                    new {id = categoryId, msg = "You have already voted for this category"});
            TempData["notice"] = "This user has already voted for this category";
            var cont = db.Contestants.Find(contestantId);
            cont.VoteCount += 1;
            db.Voters.Add(new Voter {CategoryId = categoryId, UserId = uid});
            db.Entry(cont).State = EntityState.Modified;
            db.SaveChanges();
            TempData["notice"] = "Your vote has been counted.";
            return RedirectToAction("Categories", new {id = categoryId, msg = "Your vote has been counted"});
        }

        [Authorize]
        public ActionResult VoteToo(int contestantId, int categoryId)
        {
            var uid = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(uid)) return RedirectToAction("Login", "Account");
            if (db.Voters.Any(a => a.UserId == uid && a.CategoryId == categoryId))

                return Redirect(Request.UrlReferrer.ToString());
            TempData["notice"] = "This user has already voted for this category";
            var conts = db.Contestants.Find(contestantId);
            conts.VoteCount += 1;
            db.Voters.Add(new Voter {CategoryId = categoryId, UserId = uid});
            db.Entry(conts).State = EntityState.Modified;
            db.SaveChanges();
            TempData["notice"] = "Your vote has been counted.";
            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: Contestants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contestant contestant, HttpPostedFileBase file, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid || Request.Form.AllKeys.Contains("catID"))
            {
                var id = 0;
                if (!int.TryParse(Request.Form["catID"], out id))
                {
                    ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Title");
                    return View(contestant);
                }
                contestant.Category = db.Categories.Find(id);

                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var savePath = "~/Images/" + User.Identity.GetUserId() + "/" + contestant.Name + "/";
                    var dir = new DirectoryInfo(HttpContext.Server.MapPath(savePath));


                    if (!dir.Exists)
                        dir.Create();


                    file = Request.Files[i];
                    file.SaveAs(HttpContext.Server.MapPath(savePath)
                                + file.FileName);

                    var profPath = "~/Images/ProfPath/" + User.Identity.GetUserId() + "/" + contestant.Name + "/";
                    var dir2 = new DirectoryInfo(HttpContext.Server.MapPath(profPath));

                    if (!dir2.Exists)
                        dir2.Create();

                    pic = Request.Files[i];
                    pic.SaveAs(HttpContext.Server.MapPath(profPath)
                               + pic.FileName);
                    if (contestant.Images != null)
                        contestant.Images.Add(new Media
                        {
                            Path = "/Images/" +
                                   (string.IsNullOrEmpty(User.Identity.GetUserId())
                                       ? ""
                                       : User.Identity.GetUserId() + "/") + contestant.Name + "/" + file.FileName
                        });
                    else
                        contestant.Images =
                            new List<Media>
                            {
                                new Media
                                {
                                    Path = "/Images/" +
                                           (string.IsNullOrEmpty(User.Identity.GetUserId())
                                               ? ""
                                               : User.Identity.GetUserId() + "/") + contestant.Name + "/" +
                                           file.FileName
                                }
                            };

                    if (contestant.ProfilePic != null)
                        contestant.ProfilePic.Add(new Media
                        {
                            Path = "/Images/ProfPath/" +
                                   (string.IsNullOrEmpty(User.Identity.GetUserId())
                                       ? ""
                                       : User.Identity.GetUserId() + "/") + contestant.Name + "/" + pic.FileName
                        });
                    else
                        contestant.ProfilePic =
                            new List<Media>
                            {
                                new Media
                                {
                                    Path = "/Images/ProfPath/" +
                                           (string.IsNullOrEmpty(User.Identity.GetUserId())
                                               ? ""
                                               : User.Identity.GetUserId() + "/") + contestant.Name + "/" + pic.FileName
                                }
                            };
                }

                db.Contestants.Add(contestant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Title");
            return View(contestant);
        }

        // GET: Contestants/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contestant = db.Contestants.Find(id);
            if (contestant == null)
                return HttpNotFound();
            return View(contestant);
        }

        // POST: Contestants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ID,Name,Email,PhoneNumber,DateOfBirth,Nation,States")] Contestant contestant)
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contestant = db.Contestants.Find(id);
            if (contestant == null)
                return HttpNotFound();
            return View(contestant);
        }

        // POST: Contestants/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ID,Name,Email,PhoneNumber,DateOfBirth,Nation,States")] int id)
        {
            var contestant = db.Contestants.Find(id);
            db.Contestants.Remove(contestant);
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