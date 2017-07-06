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
        [Display(Name = "Category")]
        public Category Categories { get; set; }
        public virtual ICollection<Media> Images { get; set; }

    }

    public class ContestantDbContext : DbContext
    {
        public DbSet<Contestant> Contestants { get; set; }
    }

    public class Media
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
        
    public enum Category
    {
        [Display(Name = "Music And Songs")]
        MusicAndSongs,
        Sports,
        [Display(Name = "Digital And Visual Arts")]
        DigitalAndVisualArts,
        [Display(Name = "Science And Technology")]
        ScienceAndTechnology,
        [Display(Name = "Performing Arts")]
        PerformingArts,
        [Display(Name = "Fashion Design And Modeling")]
        FashionDesignAndModeling,
        Others
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