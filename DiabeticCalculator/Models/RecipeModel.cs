using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiabeticCalculator.Models
{
    public class RecipeModel: Journal
    {
        public List<PersonalArea> Rations { get; set; }
    }
}