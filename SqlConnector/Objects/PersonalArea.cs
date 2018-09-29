using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnector.Objects
{
    public class PersonalArea: Products
    {
        public string UserID { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
