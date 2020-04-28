using CareData;
using CareModels.BarCodes;
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
    [Authorize(Roles = "Admin,Associate")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            var service = CreateCustomerService();
            var model = service.GetCusts();
            return View(model);
        }

        // Get: Customer/Create
        public ActionResult Create(bool isOrder, int barCodeId)
        {
            var userId = User.Identity.GetUserId();
            var barCodeService = new BarCodeService(userId);

            BarCode barCodeData = new BarCode();
            if (barCodeId != 0)
            {
                BarCodeDetail barCodeDetail = barCodeService.GetBarCodeById(barCodeId);
                barCodeData.BarCodeId = barCodeDetail.BarCodeId;
                barCodeData.BarCodeNumber = barCodeDetail.BarCodeNumber;
                // dummy data - don't need:
                barCodeData.CreateAt = DateTimeOffset.Now;
                barCodeData.CreateBy = "XX";
            }

            CustCreate model = new CustCreate
            {
                CustomerId = 0,
                IsOrder = isOrder,
                BarCodeId = barCodeData.BarCodeId,
                BarCode = barCodeData,
                FirstName = null,
                LastName = null,
                Address = null,
                City = null,
                State = null,
                ZipCode = 0,
                Phone = null,
                Email = null,
                NumberKids = 0
            };

            ViewBag.BarCodeId = BuildBarCodeDropdown(0);

            return View(model);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustCreate model)
        {
            int newCustId = 0;
            if (!ModelState.IsValid)
            {
                ViewBag.BarCodeId = BuildBarCodeDropdown(model.BarCodeId);
                return View(model);
            }

            var service = CreateCustomerService();

            if (model.BarCodeId == 0)
            {
                if (model.IsOrder)
                {
                    // when creating an order, BarCode can be 0
                    newCustId = service.CreateCust(model);
                    if (newCustId > 0)
                    {
                        // Order cannot exist - must create it
                        return RedirectToAction(actionName: "Create", controllerName: "Order", routeValues: new { CustId = newCustId, IsCust = true });
                    };
                }
                else
                {
                    // Creating a New Customer
                    ModelState.AddModelError("", $"BarCode cannot be 0.");

                    ViewBag.BarCodeId = BuildBarCodeDropdown(model.BarCodeId);

                    return View(model);
                }
            }

            CustDetail existingCustDetail = service.GetCustByBarCodeId(model.BarCodeId);

            if (!model.IsOrder)
            {
                // This is NOT an order, must validate that Barcode does not exist
                if (existingCustDetail.BarCodeId != null && existingCustDetail.BarCodeId != 0)
                {
                    ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

                    ViewBag.BarCodeId = BuildBarCodeDropdown(model.BarCodeId);

                    return View(model);
                }
            }

            newCustId = service.CreateCust(model);
            if (newCustId > 0)
            {
                if (model.IsOrder)
                {
                    return RedirectToAction(actionName: "Create", controllerName: "Order", routeValues: new { CustId = newCustId });
                }
                TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be created.");

            ViewBag.BarCodeId = BuildBarCodeDropdown(model.BarCodeId);

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
        public ActionResult Edit(bool isCust, bool isOrder, int id)
        {
            var service = CreateCustomerService();
            var detail = service.GetCustById(id);
            var model =
                new CustUpdate
                {
                    CustomerId = detail.CustomerId,
                    IsOrder = isOrder,
                    IsCust = isCust,
                    //BarCodeNumber = new BarCode(),
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
            var userId = User.Identity.GetUserId();
            var barCodeService = new BarCodeService(userId);

            // BarCodeId is null here if created by a Customer using Barcode Number of 0
            int barCodeIdParm = 0;
            if (detail.BarCodeId == null)
            {
                barCodeIdParm = 0;
                model.BarCodeNumber = 0;
            }
            else
            {
                barCodeIdParm = (int)model.BarCodeId;
                var barCodeDetail = barCodeService.GetBarCodeById(barCodeIdParm);
                model.BarCodeNumber = barCodeDetail.BarCodeNumber;
            }

            ViewBag.BarCodeId = BuildBarCodeDropdown(barCodeIdParm);

            return View(model);
        }

        // GET: Customer/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustUpdate model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BarCodeId = BuildBarCodeDropdown((int)model.BarCodeId);
                return View(model);
            }

            if (model.CustomerId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                ViewBag.BarCodeId = BuildBarCodeDropdown((int)model.BarCodeId);
                return View(model);
            }

            var service = CreateCustomerService();

            int barCodeIdParm = 0;
            if (model.BarCodeId != null) { barCodeIdParm = (int)model.BarCodeId; }

            CustDetail existingCustDetail = service.GetCustByBarCodeId(barCodeIdParm);
            if (existingCustDetail.BarCodeNumber != 0 && existingCustDetail.CustomerId != id)
            {
                ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

                ViewBag.BarCodeId = BuildBarCodeDropdown((int)model.BarCodeId);

                return View(model);
            }
            if (service.UpdateCust(model))
            {
                if (model.IsOrder)
                {
                    var userId = User.Identity.GetUserId();
                    var orderHeaderService = new OrderService(userId);

                    var existingOrderHeader = orderHeaderService.GetOrderHeaderByCustId(id, false);
                    if (existingOrderHeader.OrderId == 0)
                    {
                        // Order does not Exist
                        return RedirectToAction(actionName: "Create", controllerName: "Order", routeValues: new { CustId = id, IsCust = model.IsCust });
                    }
                    // Order Exists
                    if (existingOrderHeader.PullStartedAt != null)
                    {
                        TempData["SaveResult"] = "We are assembling your order now. Your order can no longer be changed.";
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction(actionName: "Edit", controllerName: "Order", routeValues: new { Id = existingOrderHeader.OrderId, IsCust = model.IsCust });
                }
                TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be updated.");

            ViewBag.BarCodeId = BuildBarCodeDropdown((int)model.BarCodeId);

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
        private IOrderedEnumerable<SelectListItem> BuildBarCodeDropdown(int selectedValue)
        {
            var userId = User.Identity.GetUserId();
            var barCodeService = new BarCodeService(userId);

            var barCodeList = new SelectList(barCodeService.GetBarCodes(), "BarCodeId", "BarCodeNumber", selectedValue);
            var sortedBarCodeList = barCodeList.OrderBy(o => o.Text);

            return sortedBarCodeList;
        }
    }
}