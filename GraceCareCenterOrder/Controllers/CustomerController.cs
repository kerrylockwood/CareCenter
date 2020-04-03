using CareModels.Customers;
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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            var service = CreateCustomerService();
            var model = service.GetCusts();
            return View(model);
        }

        //Move to OrderCreate - begin
        // GET: Customer/BarCode
        public ActionResult BarCodeDetails()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BarCodeValidate(CustBarCode model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateCustomerService();

            if (service.ValidateCustBarCode(model.BarCodeNumber) == null && model.BarCodeNumber > 0)
            {
                ModelState.AddModelError("", $"'{model.BarCodeNumber}' is not a valid Bar Code Number.  Please re-enter or contact a member of the Food Pantry team.");

                return View(model);
            };

            TempData["BarCodeId"] = model.BarCodeId;
            TempData["BarCodeNumber"] = model.BarCodeNumber;
            return RedirectToAction("Create");
        }
        //Move to OrderCreate - end

        // Get: Customer/Create
        public ActionResult Create()
        {
            ViewBag.BarCodeId = BuildBarCodeDropdown();

            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateCustomerService();

            CustDetail existingCustDetail = service.GetCustByBarCodeId(model.BarCodeId);
            if (existingCustDetail != null)
            {
                ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

                ViewBag.BarCodeId = BuildBarCodeDropdown();

                return View(model);
            }
            if (service.CreateCust(model))
            {
                TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be created.");

            ViewBag.BarCodeId = BuildBarCodeDropdown();

            return View(model);
        }

        // GET: Customer/Details
        public ActionResult Details(int id)
        {
            var svc = CreateCustomerService();
            var model = svc.GetCustById(id);

            return View(model);
        }

        // GET: Customer/Update
        public ActionResult Edit(int id)
        {
            var service = CreateCustomerService();
            var detail = service.GetCustById(id);
            var model =
                new CustUpdate
                {
                    CustomerId = detail.CustomerId,
                    BarCodeId = detail.BarCodeId,
                    FirstName = detail.FirstName,
                    LastName = detail.LastName,
                    Address = detail.Address,
                    City = detail.City,
                    State = detail.State,
                    ZipCode = detail.ZipCode,
                    Phone = detail.Phone,
                    Email = detail.Email,
                    NumberKids = detail.NumberKids
                };

            ViewBag.BarCodeId = BuildBarCodeDropdown();

            return View(model);
        }

        // GET: Customer/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustUpdate model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CustomerId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateCustomerService();

            CustDetail existingCustDetail = service.GetCustByBarCodeId(model.BarCodeId);
            if (existingCustDetail != null && existingCustDetail.BarCodeId != id)
            {
                ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

                ViewBag.BarCodeId = BuildBarCodeDropdown();

                return View(model);
            }
            if (service.UpdateCust(model))
            {
                TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be updated.");

            ViewBag.BarCodeId = BuildBarCodeDropdown();

            return View(model);
        }

        // GET: Customer/Delete
        public ActionResult Delete(int id)
        {
            var svc = CreateCustomerService();
            var model = svc.GetCustById(id);

            return View(model);
        }

        // GET: Customer/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateCustomerService();

            service.DeleteCust(id);

            TempData["SaveResult"] = $"Customer was deleted";

            return RedirectToAction("Index");
        }

        // Create Customer Service
        private CustomerService CreateCustomerService()
        {
            var userId = User.Identity.GetUserId();
            var service = new CustomerService(userId);
            return service;
        }

        // Build BarCode Dropdown
        private IOrderedEnumerable<SelectListItem> BuildBarCodeDropdown()
        {
            var userId = User.Identity.GetUserId();
            var barCodeService = new BarCodeService(userId);

            var barCodeList = new SelectList(barCodeService.GetBarCodes(), "BarCodeId", "BarCodeNumber");
            var sortedBarCodeList = barCodeList.OrderBy(o => o.Text);

            return sortedBarCodeList;
        }
    }
}