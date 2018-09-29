using Microsoft.AspNet.Identity.EntityFramework;

namespace DiabeticCalculator.Models.IdentityUs
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public string Description { get; set; }
    }
}