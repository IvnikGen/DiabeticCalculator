using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiabeticCalculator.Models.IdentityUs.CRUDUser
{
    public class EditModel
    {
        public string ID { get; set; }

        public string Login { get; set; }

        public string UserRole { get; set; }

        public string Email { get; set; }

        public int Year { get; set; }
    }
}