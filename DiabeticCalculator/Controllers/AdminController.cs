using DiabeticCalculator.Models;
using DiabeticCalculator.Models.IdentityUs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using DiabeticCalculator.Models.IdentityUs.CRUDUser;
using SqlConnector.Objects;
using SqlConnector.Methods;

namespace DiabeticCalculator.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        #region Controller
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Content()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Statistic()
        {

            List<ViewUserModel> al = new List<ViewUserModel>();
            foreach (var us in UserManager.Users)
            {
                al.Add(new ViewUserModel
                {
                    ID = us.Id,
                    AccountName = us.Login,
                    Email = us.Email,
                    DateCreate = us.DateCreate,
                    UserRole = us.UserRole
                });
            }

            List<ViewUserModelGrouped> group = al.GroupBy(x => x.DateCreate).Select(g => new ViewUserModelGrouped { DateCreate = g.Key, Count = g.Count() }).ToList();

            return PartialView(group);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult GetProductsBar()
        {
            List<Products> products = Read.getBreadUnitsTable();

            return PartialView(products);
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetAllUsPartial()
        {
            @ViewBag.User = User.IsInRole("Administrator");
            return PartialView();
        }

        [HttpPost]
        [ActionName("GetAllUsPartial")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetAllUsPartialData()
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

            List<ViewUserModel> al = new List<ViewUserModel>();
            foreach (var us in UserManager.Users)
            {
                al.Add(new ViewUserModel
                {
                    ID = us.Id,
                    AccountName = us.Login,
                    Email = us.Email,
                    DateCreate = us.DateCreate,
                    UserRole = us.UserRole
                });
            }

            var users = (from a in al select a);
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(a =>
                    a.AccountName.Contains(search) ||
                    (!string.IsNullOrEmpty(a.UserRole) && a.UserRole.Contains(search))
                    );
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                users = users.OrderBy(sortColumn + " " + sortColumnDir);
            totalRecords = users.Count();
            var data = users.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            ViewBag.Login = UserManager.FindById(id).Login;
            return PartialView();
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Register", "Account");
                }
            }
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);

                if (user != null)
                {
                    EditModel model = new EditModel { ID = user.Id, Login = user.Login, Email = user.Email, Year = user.Year, UserRole = user.UserRole };
                    return PartialView(model);
                }
            }
            catch(Exception e)
            {
                string s = e.Message;
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(EditModel model)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(model.ID);
            if (user != null)
            {
                user.Email = model.Email;
                user.Year = model.Year;
                user.UserRole = model.UserRole;

                ApplicationContext db = new ApplicationContext();
                ApplicationUser appuser = db.Users.First(x => x.Login == model.Login);

                if (appuser != null && appuser.UserRole != null)
                {
                    IdentityResult rem = UserManager.RemoveFromRole(user.Id, appuser.UserRole);
                    if (rem.Succeeded)
                    {
                        IdentityResult result = UserManager.AddToRole(user.Id, model.UserRole);
                        if (result.Succeeded)
                        {
                            await UserManager.UpdateAsync(user);
                            if (User.Identity.Name == model.Login)
                                AuthenticationManager.SignOut();
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Что-то пошло не так");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Пользователь не найден");
            }
            return RedirectToAction("Index", "Admin");
        }
        #endregion

        #region groupsTable
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetGroupsTable()
        {
            List<ProductGroups> groups = Read.getProductGroups();
            return PartialView("~/Views/Admin/Content/Groups.cshtml", groups);
        }

        [HttpPost]
        [ActionName("GetGroupsTable")]
        public ActionResult GetGroupsTableData()
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

            List<ProductGroups> groups = Read.getProductGroups();
            var users = (from a in groups select a);

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(a =>
                    (!string.IsNullOrEmpty(a.GroupName) && a.GroupName.Contains(search)) ||
                    (!string.IsNullOrEmpty(a.GroupImage) && a.GroupImage.Contains(search))
                    );
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                users = users.OrderBy(sortColumn + " " + sortColumnDir);
            totalRecords = users.Count();
            var data = users.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditGroupTable(int GroupID)
        {
            ProductGroups group = Read.getProductGroups().FirstOrDefault(x=> x.GroupID == GroupID);
            return PartialView("~/Views/Admin/Content/GroupForms/EditGroup.cshtml", group);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditGroupTable(ProductGroups data)
        {
            if(data != null)
                Update.updateProductGroups(data);

            return RedirectToAction("Content", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddGroupTable()
        {
            return PartialView("~/Views/Admin/Content/GroupForms/AddGroup.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddGroupTable(ProductGroups data)
        {
            if (data != null)
                Create.insertProductGroups(data);

            return RedirectToAction("Content", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveGroupTable(int GroupID)
        {
            ProductGroups group = Read.getProductGroups().FirstOrDefault(x => x.GroupID == GroupID);
            ViewBag.Name = group.GroupName;

            return PartialView("~/Views/Admin/Content/GroupForms/RemoveGroup.cshtml", group);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveGroupTable(ProductGroups data)
        {
            if (data != null)
                SqlConnector.Methods.Delete.deleteProductGroups(data);

            return RedirectToAction("Content", "Admin");
        }
        #endregion

        #region productsTable
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetProductsTable()
        {
            List<Products> products = Read.getBreadUnitsTable();
            return PartialView("~/Views/Admin/Content/Products.cshtml", products);
        }

        [HttpPost]
        [ActionName("GetProductsTable")]
        public ActionResult GetProductsTableData()
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

            List<Products> products = Read.getBreadUnitsTable();
            var users = (from a in products select a);

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(a =>
                    (!string.IsNullOrEmpty(a.ID.ToString()) && a.ID.ToString().Contains(search)) ||
                    (!string.IsNullOrEmpty(a.Product) && a.Product.Contains(search)) ||
                    (!string.IsNullOrEmpty(a.ProductGroupName) && a.ProductGroupName.Contains(search))
                    );
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                users = users.OrderBy(sortColumn + " " + sortColumnDir);
            totalRecords = users.Count();
            var data = users.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditProductTable(int ID)
        {
            Products product = Read.getBreadUnitsTable().FirstOrDefault(x => x.ID == ID);
            ProductModal pm = new ProductModal
            {
                ID = product.ID,
                Product = product.Product,
                ProductGroup = product.ProductGroup,
                ProductGroupName = product.ProductGroupName,
                BreadUnits = product.BreadUnits.ToString().Replace(',', '.'),
                Carbohydrates = product.Carbohydrates.ToString().Replace(',', '.'),
                GrammInUnit = product.GrammInUnit
            };

            List<string> groups = new List<string>();
            Read.getProductGroups().ForEach(x => groups.Add(x.GroupName));

            ViewBag.Groups = new SelectList(groups);

            return PartialView("~/Views/Admin/Content/ProductForms/EditProduct.cshtml", pm);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditProductTable(ProductModal data)
        {
            Products prod = new Products
            {
                ID = data.ID,
                Product = data.Product,
                ProductGroupName = data.ProductGroupName,
                BreadUnits = Convert.ToDouble(data.BreadUnits.Replace('.', ',')),
                Carbohydrates = Convert.ToDouble(data.Carbohydrates.Replace('.', ',')),
                GrammInUnit = data.GrammInUnit,
                ProductGroup = Read.getProductGroups().First(x => x.GroupName == data.ProductGroupName).GroupID
            };

            if (data != null)
                Update.updateBreadUnitsTable(prod);

            return RedirectToAction("Content", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddProductTable()
        {
            List<string> groups = new List<string>();
            Read.getProductGroups().ForEach(x=> groups.Add(x.GroupName));

            ViewBag.Groups = new SelectList(groups);

            return PartialView("~/Views/Admin/Content/ProductForms/AddProduct.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddProductTable(ProductModal data)
        {
            Products prod = new Products
            {
                Product = data.Product,
                ProductGroupName = data.ProductGroupName,
                BreadUnits = Convert.ToDouble(data.BreadUnits.Replace('.', ',')),
                Carbohydrates = Convert.ToDouble(data.Carbohydrates.Replace('.', ',')),
                GrammInUnit = data.GrammInUnit,
                ProductGroup = Read.getProductGroups().First(x => x.GroupName == data.ProductGroupName).GroupID
            };

            if (data != null)
                Create.insertProduct(prod);

            return RedirectToAction("Content", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveProductTable(int ID)
        {
            Products product = Read.getBreadUnitsTable().FirstOrDefault(x => x.ID == ID);
            ViewBag.Name = product.Product;

            return PartialView("~/Views/Admin/Content/ProductForms/RemoveProduct.cshtml", product);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveProductTable(Products data)
        {
            if (data != null)
                SqlConnector.Methods.Delete.deleteBreadUnitsTable(data);

            return RedirectToAction("Content", "Admin");
        }
        #endregion
    }
}