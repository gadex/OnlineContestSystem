using System;
using System.Linq;
using System.Web.Mvc;

using OnlineContestSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineContestSystem.CustomFilters;
using OnlineContestSystem.ViewModels;

namespace OnlineContestSystem.Controllers
{
    [AuthLog(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext context;
 
        public RoleController()
        {
            context = new ApplicationDbContext(); 
        }

        private readonly ContestantDbContext db = new ContestantDbContext();
        
        [Route]
        public ActionResult Index()
        {
            return View("Index", new MessageReplyViewModel()
            {
                Contestants = db.Contestants.OrderByDescending(x => x.ID).Take(10).ToList(),
                Categories = db.Categories.ToList(),
                KnownTalents = db.KnownTalents.ToList(),
                Blogs = db.Blogs.ToList()
            });
        }
 
        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }
 
        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
     
 
    }
}