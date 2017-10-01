using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineContestSystem.Models;
using OnlineContestSystem.ViewModels;
using SendGrid;

namespace OnlineContestSystem.Controllers
{
    public class MessageController : Controller
    {
        private readonly ContestantDbContext db = new ContestantDbContext();
        [HttpPost]
        public ActionResult Create(MessageReplyViewModel vm, string Subject, string MessageToPost)
        {
            var username = User.Identity.Name;
            string fullName = "";
            int msgid = 0;
            if (!string.IsNullOrEmpty(username))
            {
                fullName = username;
            }
            Message messagetoPost = new Message();
            if (Subject != string.Empty && MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = Subject;
                messagetoPost.MessageToPost = MessageToPost;
                messagetoPost.From = fullName;

                db.Messages.Add(messagetoPost);
                db.SaveChanges();
                msgid = messagetoPost.Id;
            }

            //return RedirectToAction("Index", "Contestants", new { Id = msgid });
            //return PartialView("_ChallengeSection", vm);
            //viewModel=your viewModel
            //return PartialView("_ChallengeSection", vm);

            return Json("success", JsonRequestBehavior.AllowGet);

        }

        public ActionResult Create()
        {
            MessageReplyViewModel vm = new MessageReplyViewModel();

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ReplyMessage(MessageReplyViewModel vm, int messageId, string replyMessage, HttpPostedFileBase file)
        {
            try
            {

            var username = User.Identity.Name;
            string fullName = "";
            if (!string.IsNullOrEmpty(username))
            {

                fullName = username;
            }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string savePath = "~/Images/Replies" + User.Identity.Name + "/";
                    DirectoryInfo dir = new DirectoryInfo(HttpContext.Server.MapPath(savePath));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    file = Request.Files[i];
                    file.SaveAs(HttpContext.Server.MapPath(savePath)
                                + file.FileName);
                    if (vm.Reply.ReplyVideo != null)
                        vm.Reply.ReplyVideo.Add(new Models.Media
                        {
                            Path = "/Images/Replies" + User.Identity.Name + "/" + file.FileName
                        });
                    else
                        vm.Reply.ReplyVideo = new List<Models.Media>
                        {
                            new Models.Media {Path = "/Images/Replies" + User.Identity.Name + "/" + file.FileName}
                        };



                }

                Reply _reply = new Reply();
                    _reply.ReplyDateTime = DateTime.Now;
                    _reply.MessageId = messageId;
                    _reply.ReplyFrom = fullName;
                    _reply.ReplyMessage = vm.Reply.ReplyMessage;
                _reply.ReplyVideo = vm.Reply.ReplyVideo;
                    db.Replies.Add(_reply);
                    db.SaveChanges();
                

            }
        catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }

            //reply to the message owner          - using email template

            var messageOwner = db.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            //var users = from user in db.Users
            //    orderby user.FirstName
            //    select new
            //    {
            //        FullName = user.FirstName + " " + user.LastName,
            //        UserEmail = user.Email
            //    };

            //var uemail = User.Identity.GetUserName().Where(x => x.ToString() == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
            //SendGridMessage ReplyMessage = new SendGridMessage();
            //ReplyMessage.From = new MailAddress(username);
            //ReplyMessage.Subject = "Reply for your message :" + db.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault();
            //ReplyMessage.Text = vm.Reply.ReplyMessage;


            //ReplyMessage.AddTo(uemail);


            //var credentials = new NetworkCredential("YOUR SENDGRID USERNAME", "PASSWORD");
            //var transportweb = new Web(credentials);
            //transportweb.DeliverAsync(ReplyMessage);
            //return RedirectToAction("Index", "Contestants", new { Id = messageId });
            return Json("success", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteMessage(int messageId)
        {
            Message _messageToDelete = db.Messages.Find(messageId);
            db.Messages.Remove(_messageToDelete);
            db.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = db.Replies.Where(i => i.MessageId == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    db.Replies.Remove(rep);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Index", "Contestants");
        }
    }
}