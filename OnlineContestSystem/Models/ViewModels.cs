using System;
using System.Collections.Generic;

namespace OnlineContestSystem.Models
{
    //public class VoteIndexViewModel
    //{
    //    public List<Category> Categories { get; set; }
    //    public List<Contestant> Contestants { get; set; }
    //    public List<Blog> Blogs { get; set; }
    //    public List<KnownTalents> KnownTalents { get; set; }
    //}


    public class VoteCategoryViewModel
    {
        public Category Category { get; set; }
        public List<Contestant> Contestants { get; set; }
    }
}