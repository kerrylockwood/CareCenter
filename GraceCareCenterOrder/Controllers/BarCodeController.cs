using CareModels.BarCodes;
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
    public class BarCodeController : Controller
    {
        // GET: BarCode
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new BarCodeService(userId);
            var model = service.GetBarCodes();
            return View(model);
        }

        // Get: BarCode/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BarCode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BarCodeCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateBarCodeService();

            if (service.CreateBarCode(model))
            {
                TempData["SaveResult"] = $"BarCode {model.BarCodeNumber} was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"{model.BarCodeNumber} could not be created.");

            return View(model);
        }

        // GET: BarCode/Delete
        public ActionResult Delete(int id)
        {
            var svc = CreateBarCodeService();
            var model = svc.GetBarCodeById(id);

            return View(model);
        }

        // GET: BarCode/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateBarCodeService();

            service.DeleteBarCode(id);

            TempData["SaveResult"] = $"BarCode was deleted";

            return RedirectToAction("Index");
        }

        // Create BarCode Service
        private BarCodeService CreateBarCodeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new BarCodeService(userId);
            return service;
        }
    }
}