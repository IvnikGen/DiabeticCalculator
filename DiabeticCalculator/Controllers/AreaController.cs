using SqlConnector.Methods;
using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity;
using DiabeticCalculator.Models.IdentityUs;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace DiabeticCalculator.Controllers
{
    public class AreaController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Area
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult FromCalc()
        {
            List<Products> products = TempData["calcTable"] as List<Products>;

            List<PersonalArea> persarea = new List<PersonalArea>();

            try
            {
                var user = UserManager.FindByIdAsync(User.Identity.GetUserId());

                foreach (var prod in products)
                {
                    persarea.Add(new PersonalArea
                    {
                        Product = prod.Product,
                        ProductGroup = prod.ProductGroup,
                        DateCreate = DateTime.Now,
                        ProductGroupName = prod.ProductGroupName,
                        Carbohydrates = prod.Carbohydrates,
                        BreadUnits = prod.BreadUnits,
                        GrammInUnit = prod.GrammInUnit,
                        UserID = user.Result.Login
                    });
                }

                foreach (var item in persarea)
                {
                    Create.insertArea(item);
                }
            

                return View("~/Views/Area/Index.cshtml");
            }
            catch
            {
                return View("~/Views/Area/Index.cshtml");
            }

        }

        [Authorize]
        public ActionResult Charts()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userID = user.Login;
            List<PersonalArea> persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList();

            return View(persarea);
        }

        [Authorize]
        public ActionResult GetTableDataArea()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userID = user.Login;

            List<PersonalArea> persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList();
            @ViewBag.User = userID;
            return PartialView("~/Views/Area/CalculateTableArea.cshtml", persarea);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetTableDataAreaSort(string userID)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            List<PersonalArea> persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList();

            var products = (from a in persarea select a);
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(a =>
                    a.ProductGroupName.Contains(search) ||
                    a.Product.Contains(search)
                    );
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                products = products.OrderBy(sortColumn + " " + sortColumnDir);

            totalRecords = products.Count();
            var data = products.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetGraphArea()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userID = user.Login;

            List<PersonalArea> persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList();
            return PartialView("~/Views/Area/MainGraph.cshtml", persarea);

        }

        [HttpGet]
        public ActionResult TableDelete(int id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userID = user.Login;

            PersonalArea persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList().First(y=> y.ID == id);
            @ViewBag.ProdName = persarea.Product;
            return PartialView();
        }

        [HttpPost]
        [ActionName("TableDelete")]
        public ActionResult TableDeleteConfirmed(int id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userID = user.Login;

            try
            {
                PersonalArea persarea = Read.getAreaTable().Where(x => x.UserID == userID).ToList().First(y => y.ID == id);
                Delete.deleteAreaTable(persarea);
            }
            catch { }

            return View("~/Views/Area/Index.cshtml");
        }

    }
}