using CareData;
using GraceCareCenterOrder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace GraceCareCenterOrder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(string sortOrder, string searchData, string filterValue, int? pageNum)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : "";
            ViewBag.SortRole = String.IsNullOrEmpty(sortOrder) ? "Role_Ascend" : "Role_Desc";

            if (searchData != null)
            {
                pageNum = 1;
            }
            else
            {
                searchData = filterValue;
            }

            ViewBag.FilterValue = searchData;

            ApplicationDbContext ctx = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

            List<UserRole> model = new List<UserRole>();

            var userList = ctx.Users.ToList();
            foreach (var usr in userList)
            {
                UserRole usrRole = new UserRole();
                usrRole.UserId = usr.Id;
                usrRole.UserName = usr.UserName;

                var roleList = UserManager.GetRolesAsync(usr.Id).Result;

                if (roleList.Count > 0)
                {
                    string comma = null;
                    foreach (var roleItem in roleList)
                    {
                        usrRole.Role = usrRole.Role + comma + roleItem;
                        comma = ", ";
                    }
                }
                else
                {
                    usrRole.Role = "none";
                }
                model.Add(usrRole);
            }

            if (!String.IsNullOrEmpty(searchData))
            {
                model = model.Where(m => m.UserName.ToUpper().Contains(searchData.ToUpper()) || m.Role.ToUpper().Contains(searchData.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "Name_Desc":
                    model = model.OrderByDescending(m => m.UserName).ToList();
                    break;
                case "Role_Ascend":
                    model = model.OrderBy(m => m.Role).ThenBy(m => m.UserName).ToList();
                    break;
                case "Role_Desc":
                    model = model.OrderByDescending(m => m.Role).ThenByDescending(m => m.UserName).ToList();
                    break;
                default:
                    model = model.OrderBy(m => m.UserName).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (pageNum ?? 1);

            return View(model.ToPagedList(noOfPage, sizeOfPage));
        }

        // GET: Users/Update
        public ActionResult Edit(string userId)
        {
            UserRole model = new UserRole();
            ApplicationDbContext context = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            model.UserName = UserManager.FindById(userId).UserName;
            var s = UserManager.GetRoles(userId);
            model.Role = s[0];

            List<RoleItem> roleItemList = new List<RoleItem>();
            string selectedRoleId = null;

            foreach (var roleItm in context.Roles)
            {
                RoleItem roleItem = new RoleItem();
                roleItem.RoleId = roleItm.Id;
                roleItem.Role = roleItm.Name;
                if (model.Role == roleItm.Name) { selectedRoleId = roleItm.Id; }
                roleItemList.Add(roleItem);
            }

            var rolesList = new SelectList(roleItemList, "RoleId", "Role", selectedRoleId);

            ViewBag.RoleId = rolesList;

            return View(model);
        }

        // GET: Users/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRole model)
        {
            if (!ModelState.IsValid) return View(model);

            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationDbContext contextRole = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var entity = context.Roles.Single(m => m.Id == model.RoleId);

            foreach (var roleItm in contextRole.Roles)
            {
                var result1 = userManager.RemoveFromRole(model.UserId, roleItm.Name);
            }

            var result2 = userManager.AddToRole(model.UserId, entity.Name);

            if (result2.Succeeded)
            {
                TempData["SaveResult"] = $"{model.UserName} was updated.";
            }
            else
            {
                TempData["SaveResult"] = $"Update to {model.UserName} failed.";
            }

            return RedirectToAction("Index");
        }
    }
}