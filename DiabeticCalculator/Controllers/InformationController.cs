using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using SqlConnector.Methods;

namespace DiabeticCalculator.Controllers
{
    public class InformationController : Controller
    {
        public static List<Products> currentTableData;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print()
        {
            List<Products> buTable = Read.getBreadUnitsTable().ToList();
            currentTableData = buTable;

            return PartialView("Print", buTable);
        }

        [HttpPost]
        public ActionResult PrintData()
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

            var products = (from a in currentTableData select a);
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