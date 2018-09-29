using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiabeticCalculator.Models
{
    public class ProductModal
    {
        public int ID { get; set; }

        public string Product { get; set; }

        public int ProductGroup { get; set; }

        public string Carbohydrates { get; set; }

        public string BreadUnits { get; set; }

        public int GrammInUnit { get; set; }

        public string ProductGroupName { get; set; }
    }
}