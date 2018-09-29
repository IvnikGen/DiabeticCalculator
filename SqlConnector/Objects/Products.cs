using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnector.Objects
{
    public class Products
    {
        public int ID { get; set; }

        public string Product { get; set; }

        public int ProductGroup { get; set; }

        public double Carbohydrates { get; set; }

        public double BreadUnits { get; set; }

        public int GrammInUnit { get; set; }

        public string ProductGroupName { get; set; }
    }
}
