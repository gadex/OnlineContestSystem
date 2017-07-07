using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineContestSystem.Models
{
    public class VoteIndexViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Contestant> Contestants { get; set; }
    }


    public class VoteCategoryViewModel
    {
        public Category Categorey { get; set; }
        public List<Contestant> Contestants { get; set; }
    }

}