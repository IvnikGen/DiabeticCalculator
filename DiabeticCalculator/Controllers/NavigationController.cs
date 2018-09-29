using SqlConnector.Methods;
using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiabeticCalculator.Controllers
{
    public class NavigationController : Controller
    {

        public ActionResult GetGroupList()
        {
           return PartialView(getGroupsRandom());
        }

        public ActionResult GetGroupListAll()
        {
           return PartialView(getGroups());
        }

        private List<ProductGroups> getGroups()
        {
            List<ProductGroups> groups = Read.getProductGroups();

            return groups;
        }

        private List<ProductGroups> getGroupsRandom()
        {
            List<ProductGroups> groups = Read.getProductGroups();
            List<ProductGroups> random = new List<ProductGroups>();

            Random rand = new Random();
            List<int> nums = new List<int>();

            while (nums.Count <= 12)
            {
                int num = rand.Next(0, groups.Count - 1);
                if(!nums.Contains(num))
                    nums.Add(num);
            }

            foreach (var index in nums)
                random.Add(groups[index]);
            
            return random;
        }
    }
}