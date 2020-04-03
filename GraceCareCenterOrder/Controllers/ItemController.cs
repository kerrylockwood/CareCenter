using CareData;
using CareModels.Items;
using CareServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace GraceCareCenterOrder.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {

        // GET: Item
        public ActionResult Index()
        {
            var service = CreateItemService();
            var model = service.GetItems();

            return View(model.OrderBy(o => o.CategoryName).ThenBy(o => o.SubCatName).ThenBy(o => o.ItemName));
        }

        // Get: Item/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var subCatService = new SubCatService(userId);

            var sortedCatSubCatList = new SelectList(subCatService.GetCatSubCats(), "SubCatId", "CatSubCatName");
            var sortedCatagoryList = sortedCatSubCatList.OrderBy(o => o.Text);

            ViewBag.SubCatId = sortedCatSubCatList;

            //Type type = typeof(Item);
            //PropertyInfo[] propertyInfo = type.GetProperties();

            //foreach (PropertyInfo pInfo in propertyInfo)
            //{
            //    string test = pInfo.Name;
            //}
            //var test = "xx";
            //var xxx = GetPropertyName(item.);
            //Expression<Func<CareData.Item, System.ComponentModel.DataAnnotations.DisplayAttribute>> expression = i => i.MaxAllowed;
            //var name = GetPropertyName<DisplayAttribute>(expression);
            //var maxItems = CareData.Item

            return View();
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateItemService();

            if (service.CreateItem(model))
            {
                TempData["SaveResult"] = $"'{model.ItemName}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.ItemName}' could not be created.");

            return View(model);
        }

        // GET: Item/details
        public ActionResult Details(int id)
        {
            var service = CreateItemService();
            var model = service.GetItemById(id);

            return View(model);
        }

        // GET: Item/Update
        public ActionResult Edit(int id)
        {
            var service = CreateItemService();
            var detail = service.GetItemById(id);
            var model =
                new ItemUpdate
                {
                    ItemId = detail.ItemId,
                    SubCatId = detail.SubCatId,
                    ItemName = detail.ItemName,
                    IsleNumber = detail.IsleNumber,
                    MaxAllowed = detail.MaxAllowed,
                    PointCost = detail.PointCost
                };

            var userId = User.Identity.GetUserId();
            var subCatService = new SubCatService(userId);

            var sortedCatSubCatList = new SelectList(subCatService.GetCatSubCats(), "SubCatId", "CatSubCatName", model.SubCatId);
            var sortedCatagoryList = sortedCatSubCatList.OrderBy(o => o.Text);

            ViewBag.SubCatId = sortedCatSubCatList;
            return View(model);
        }

        // GET: Item/update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(int id, ItemUpdate model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.ItemId != id)
            {
                ModelState.AddModelError("", "ID mismatch");
                return View(model);
            }

            var service = CreateItemService();

            if (service.UpdateItem(model))
            {
                TempData["SaveResult"] = $"'{model.ItemName}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.ItemName}' could not be updated.");
            return View(model);
        }

        // GET: Item/Delete
        public ActionResult Delete(int id)
        {
            var service = CreateItemService();
            var model = service.GetItemById(id);

            return View(model);
        }

        // GET: Item/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateItemService();

            service.DeleteItem(id);

            TempData["SaveResult"] = $"Item was deleted";

            return RedirectToAction("Index");
        }

        // Create Item Service
        private ItemService CreateItemService()
        {
            var userId = User.Identity.GetUserId();
            var service = new ItemService(userId);
            return service;
        }

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression propertyExpression = (MemberExpression)expression.Body;
            MemberInfo propertyMember = propertyExpression.Member;

            Object[] displayAttributes = propertyMember.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (displayAttributes != null && displayAttributes.Length == 1)
                return ((DisplayAttribute)displayAttributes[0]).Name;

            return propertyMember.Name;
        }
    }
}