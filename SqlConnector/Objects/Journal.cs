using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnector.Objects
{
    public class Journal
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public double Insulin { get; set; }

        public double SugarLevel { get; set; }

        public string SugarLevelS { get; set; }
    }
}
