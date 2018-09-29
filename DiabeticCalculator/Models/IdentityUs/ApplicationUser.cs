using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace DiabeticCalculator.Models.IdentityUs
{
    public class ApplicationUser : IdentityUser
    {
        public string Login { get; set; }

        public DateTime? DateCreate { get; set; }

        public string UserRole { get; set; }

        public int Year { get; set; }

        public int RatioID { get; set; }

        public int JournalID { get; set; }
    }

}