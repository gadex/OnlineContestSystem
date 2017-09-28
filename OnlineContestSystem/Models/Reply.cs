using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineContestSystem.Models
{
    public class Reply
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string ReplyFrom { get; set; }
        public string ReplyMessage { get; set; }
        public virtual ICollection<Media> ReplyVideo { get; set; }
        public DateTime ReplyDateTime { get; set; }
    }
}