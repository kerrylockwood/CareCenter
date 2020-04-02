using CareModels.Categories;
using CareServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceCareCenterOrder.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var model = service.GetCategories();
            return View(model.OrderBy(o => o.CategoryName));
        }

        // Get: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateCategoryService();

            if (service.CreateCategory(model))
            {
                TempData["SaveResult"] = $"'{model.CategoryName}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.CategoryName}' could not be created.");

            return View(model);
        }

        // GET: Category/Details
        public ActionResult Details(int id)
        {
            var svc = CreateCategoryService();
            var model = svc.GetCategoryById(id);

            return View(model);
        }

        // GET: Category/Update
        public ActionResult Edit(int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetCategoryById(id);
            var model =
                new CategoryUpdate
                {
                    CategoryId = detail.CategoryId,
                    CategoryName = detail.CategoryName
                };
            return View(model);
        }

        // GET: Category/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryUpdate model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CategoryId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateCategoryService();

            if (service.UpdateCategory(model))
            {
                TempData["SaveResult"] = $"'{model.CategoryName}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.CategoryName}' could not be updated.");
            return View(model);
        }

        // GET: Category/Delete
        public ActionResult Delete(int id)
        {
            var svc = CreateCategoryService();
            var model = svc.GetCategoryById(id);

            return View(model);
        }

        // GET: Category/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateCategoryService();

            service.DeleteCategory(id);

            TempData["SaveResult"] = $"Category was deleted";

            return RedirectToAction("Index");
        }

        // Create Category Service
        private CategoryService CreateCategoryService()
        {
            var userId = User.Identity.GetUserId();
            var service = new CategoryService(userId);
            return service;
        }
    }
}