using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineContestSystem.Models;
using SendGrid;

namespace OnlineContestSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContestantDbContext db = new ContestantDbContext();

        public ActionResult About()
        {
            ViewBag.Message = "About Talent Unleash";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactUs contactUs)
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress(contactUs.Email, contactUs.Name);

            // Add multiple addresses to the To field.
            myMessage.AddTo("info@talentunleash.com");

            myMessage.Subject = contactUs.Subject;

            //Add the HTML and Text bodies
            //myMessage.Html = "<p></p>";
            myMessage.Text = contactUs.Message;

            var credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]
            );
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
                
            }
            else
            {
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partner()
        {
            ViewBag.Message = "Become A Partner";

            return View();
        }

        public ActionResult HowItWorks()
        {
            ViewBag.Message = "How to Win";

            return View();
        }
    }
}