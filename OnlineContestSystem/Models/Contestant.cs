using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace OnlineContestSystem.Models
{
    public class Contestant
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Profile Picture")]
        public virtual ICollection<Media> ProfilePic { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public Gender Genders { get; set; }

        [Display(Name = "Describe your talent")]
        public string Bio { get; set; }

        [Display(Name = "Upload Images")]
        public virtual ICollection<Media> Images { get; set; }

        public virtual Category Category { get; set; }

        //The total number of votes
        public int VoteCount { get; set; }
    }

    /// <summary>
    ///     Let this, help check for double voting in a category
    /// </summary>
    public class Voter
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
    }


    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Media> CatImage { get; set; }
    }

    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Media> BlogImage { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
    }

    public class KnownTalents
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Media> TalentImage { get; set; }
    }

    public class ContactUs
    {
        [Required]
        [StringLength(100)]
        [Display(Order = 1)]
        public virtual string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        [Display(Order = 2)]
        public virtual string Email { get; set; }

        [Required]
        [StringLength(254)]
        [Display(Order = 3)]
        public virtual string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [Display(Order = 4)]
        public virtual string Message { get; set; }
    }

    public class ChallengeVideo
    {
        public int Id { get; set; }
        public virtual ICollection<Media> ChallengeVid { get; set; }
        public DateTime UploadDate { get; set; }
    }

    public class ContestantDbContext : DbContext
    {
        public DbSet<Contestant> Contestants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<KnownTalents> KnownTalents { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<ChallengeVideo> ChallengeVideos { get; set; }

        public System.Data.Entity.DbSet<OnlineContestSystem.Models.Media> Media { get; set; }
    }

    public class Media
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}