using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiabeticCalculator.Models
{
    public class ViewUserModel
    {
        public string ID { get; set; }

        public string AccountName { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }

        public DateTime? DateCreate { get; set; }
    }
}