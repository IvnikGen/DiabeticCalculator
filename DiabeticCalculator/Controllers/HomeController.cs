using SqlConnector.Methods;
using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity;


namespace DiabeticCalculator.Controllers
{
    public class HomeController : Controller
    {
        //public static List<int> calcIDs = new List<int>();
        public static List<Products> calcIDs = new List<Products>();

        // GET: Home
        public ActionResult Index()
        {
            SessionCurrent.Current.CurentBreadUnitsList = new List<Products>();
            calcIDs = SessionCurrent.Current.CurentBreadUnitsList;

            return View();
        }

        public ActionResult GetTableData(string groupName)
        {
            List<Products> buTable = Read.getBreadUnitsTable().Where(x => x.ProductGroupName == groupName).ToList();
            @ViewBag.Group = groupName;
            return PartialView("~/Views/Home/DefaultTable.cshtml", buTable);
        }

        [HttpPost]
        public ActionResult GetTableDataSort(string groupName)
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

            switch (groupName)
            {
                case "Бобовые":
                    {
                        List<Products> buTable = Read.getBreadUnitsTable().Where(x => x.ProductGroupName == groupName).ToList();

                        var products = (from a in buTable select a);
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
                default:
                    {
                        List<Products> buTable = Read.getBreadUnitsTable().Where(x => x.ProductGroupName == groupName).ToList();

                        var products = (from a in buTable select a);
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
            }

        }


        public ActionResult GetTableDataAdmin(int ProductID = -1, int gramm=100)
        {
            List<Products> buTable = Read.getBreadUnitsTable().Where(x => x.ID == ProductID).ToList();
            Products prodToSearch = calcIDs.Find(x => x.ID == ProductID);

            if (prodToSearch != null)
            {
                if (prodToSearch.GrammInUnit == gramm)
                    calcIDs.Remove(calcIDs.Find(x => x.ID == ProductID));
                else
                {
                    double norm = prodToSearch.BreadUnits;

                    if (gramm< prodToSearch.GrammInUnit)
                    {
                        prodToSearch.BreadUnits = Math.Round((norm * gramm / 100), 2);
                        prodToSearch.Carbohydrates = Math.Round((norm * gramm / 100) * 12, 2);
                    }
                    else
                    {
                        prodToSearch.BreadUnits = Math.Round(gramm * norm / prodToSearch.GrammInUnit, 2);
                        prodToSearch.Carbohydrates = Math.Round((gramm * norm / prodToSearch.GrammInUnit) * 12, 2);
                    }

                    prodToSearch.GrammInUnit = gramm;
                }
            }
            else
            {
                foreach (var item in buTable)
                {

                    item.GrammInUnit = gramm;
                    calcIDs.Add(item);
                }
            }
                

            
            @ViewBag.ID = ProductID;
            @ViewBag.gramm = gramm;

            return PartialView("~/Views/Home/CalculateTable.cshtml", calcIDs);
        }

        [HttpPost]
        public ActionResult GetTableDataAdminSort(int ProductID, int gramm)
        {
            try
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

                List<Products> buTable = new List<Products>();

                //if (gramm != 100)
                //{
                //    buTable = Read.getBreadUnitsTable().Where(x => x.ID == ProductID).ToList();
                //    buTable.FirstOrDefault().GrammInUnit = gramm;
                //}
                //else
                //    buTable = Read.getBreadUnitsTable().Where(x => x.ID == calcIDs.FirstOrDefault(y => y.ID == ProductID).ID ).ToList();


                var products = (from a in calcIDs select a);
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
            catch
            {
                var draw = 1;
                var search = "";

                var sortColumn = "";
                var sortColumnDir = "";

                int pageSize = 0;
                int skip = 0;
                int totalRecords = 0;

                List<Products> buTable = Read.getBreadUnitsTable().Where(x => x.ID == ProductID).ToList();
                if (gramm != 0)
                    buTable.FirstOrDefault().GrammInUnit = gramm;

                var products = (from a in buTable select a);
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
        }


        [Authorize]
        public ActionResult SendToArea()
        {
            if(calcIDs.Count == 0)
                return PartialView("~/Views/Home/CalculateTable.cshtml", calcIDs);

            List<Products> products = calcIDs;
            TempData["calcTable"] = products;

            return RedirectToAction("FromCalc", "Area");
        }
    }
}
