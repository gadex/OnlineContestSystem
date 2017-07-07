using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OnlineContestSystem.Models
{
    public class Contestant
    {
        public int ID { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Nationality")]
        public Nationality Nation { get; set; }
        [Display(Name = "State of Origin")]
        public State States { get; set; }
        public virtual ICollection<Media> Images { get; set; }
        public virtual Category Category { get; set; }
        //The total number of votes
        public int VoteCount { get; set; }

    }

    /// <summary>
    /// Let this, help check for double voting in a categorey
    /// </summary>
    public class Voter
    {
        public int Id { get; set; }
        public int CategoreyId { get; set; }
        public string UserId { get; set; }
    }


    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }

    public class ContestantDbContext : DbContext
    {
        public DbSet<Contestant> Contestants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Voter> Voters { get; set; }
    }

    public class Media
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
        
   
    public enum Nationality
    {
        Nigerian,Other
    }

    public enum State
    {
        Abia,
        Adamawa,
        [Display(Name = "Akwa Ibom")]
        AkwaIbom,
        Anambra,
        Bauchi,
        Bayelsa,
        Benue,
        Borno,
        [Display(Name = "Cross River")]
        CrossRiver,
        Delta,
        Ebonyi,
        Edo,
        Ekiti,
        Enugu,
        [Display(Name = "Federal Capital Territory")]
        Fct,
        Gombe,
        Imo,
        Jigawa,
        Kaduna,
        Kano,
        Katsina,
        Kebbi,
        Kogi,
        Kwara,
        Lagos,
        Nasarawa,
        Niger,
        Ogun,
        Ondo,
        Osun,
        Oyo,
        Plateau,
        Rivers,
        Sokoto,
        Taraba,
        Yobe,
        Zamfara
    }

//     public static class EnumExtensions
//{
//    public static string GetDisplayName(this Enum enumValue)
//    {
//        return enumValue.GetType()
//                        .GetMember(enumValue.ToString())
//                        .First()
//                        .GetCustomAttribute<DisplayAttribute>()
//                        .GetName();
//    }
//}


}