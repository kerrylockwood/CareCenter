﻿using CareData;
using CareModels.SubCategories;
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
    public class SubCatController : Controller
    {
        // GET: SubCategory
        public ActionResult Index()
        {
            var service = CreateSubCatService();
            var model = service.GetSubCats();

            return View(model.OrderBy(o => o.CategoryName).ThenBy(o => o.SubCatName));
        }

        // Get: SubCategory/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var catService = new CategoryService(userId);

            var newCategoryList = new SelectList(catService.GetCategories(), "CategoryId", "CategoryName");
            var sortedCatagoryList = newCategoryList.OrderBy(o => o.Text);

            ViewBag.CategoryId = sortedCatagoryList;
            return View();
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubCatCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateSubCatService();

            if (service.CreateSubCat(model))
            {
                TempData["SaveResult"] = $"'{model.SubCatName}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.SubCatName}' could not be created.");

            return View(model);
        }

        // GET: Subcategory/details
        public ActionResult Details(int id)
        {
            var service = CreateSubCatService();
            var model = service.GetSubCatById(id);

            return View(model);
        }

        // GET: SubCategory/Update
        public ActionResult Edit(int id)
        {
            var service = CreateSubCatService();
            var detail = service.GetSubCatById(id);
            var model =
                new SubCatUpdate
                {
                    SubCatId = detail.SubCatId,
                    CategoryId = detail.CategoryId,
                    SubCatName = detail.SubCatName,
                    SubCatMaxAllowed = detail.SubCatMaxAllowed
                };

            var userId = User.Identity.GetUserId();
            var catService = new CategoryService(userId);

            var newCategoryList = new SelectList(catService.GetCategories(), "CategoryId", "CategoryName", model.CategoryId);
            var sortedCatagoryList = newCategoryList.OrderBy(o => o.Text);

            ViewBag.CategoryId = sortedCatagoryList;
            return View(model);
        }

        // GET: SubCategory/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SubCatUpdate model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.SubCatId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateSubCatService();

            if (service.UpdateSubCat(model))
            {
                TempData["SaveResult"] = $"'{model.SubCatName}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.SubCatName}' could not be updated.");
            return View(model);
        }

        // GET: SubCategory/Delete
        public ActionResult Delete(int id)
        {
            var service = CreateSubCatService();
            var model = service.GetSubCatById(id);

            return View(model);
        }

        // GET: SubCategory/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateSubCatService();

            service.DeleteSubCat(id);

            TempData["SaveResult"] = $"SubCategory was deleted";

            return RedirectToAction("Index");
        }

        // Create SubCategory Service
        private SubCatService CreateSubCatService()
        {
            var userId = User.Identity.GetUserId();
            var service = new SubCatService(userId);
            return service;
        }
    }
}