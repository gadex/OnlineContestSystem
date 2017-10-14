using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineContestSystem.Models;

namespace OnlineContestSystem.ViewModels
{
    public class MessageReplyViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Contestant> Contestants { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<KnownTalents> KnownTalents { get; set; }
        public List<ChallengeVideo> ChallengeVideos{ get; set; }

        private List<MessageReply> _replies = new List<MessageReply>();
        public Reply Reply { get; set; }

        public Message Message { get; set; }

        public List<MessageReply> Replies
        {
            get { return _replies; }
            set { _replies = value; }
        }

        public PagedList.IPagedList<Message> Messages { get; set; }

        public class MessageReply
        {
            public int Id { get; set; }
            public int MessageId { get; set; }
            public string MessageDetails { get; set; }
            public string ReplyFrom { get; set; }
            public string ReplyMessage { get; set; }
            public virtual ICollection<Media> ReplyVideo { get; set; }
            public DateTime ReplyDateTime { get; set; }
        }
    }
}