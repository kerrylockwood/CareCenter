using CareModels.BarCodes;
using CareModels.Customers;
using CareModels.Orders;
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
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var service = CreateOrderService();
            var model = service.GetOrders();

            // Get the Appointment Date/Time for each Order
            foreach (var item in model)
            {
                item.SlotDateTime = service.ConvertSlotToDateTime(item.SlotId, item.CreateDateTime.DateTime, item.Deliver, userId);
            }

            return View(model);
        }

        // OrderCreate Stream - Starts here
        // GET: Customer/BarCode
        public ActionResult GetCustBarCode(bool isCust)
        {
            OrderGetCustBarCode model = new OrderGetCustBarCode();
            model.BarCodeNumber = -1;
            model.IsCust = isCust;
            return View(model);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCustBarCode(OrderGetCustBarCode model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.BarCodeNumber > 0)
            {
                var userId = User.Identity.GetUserId();
                var BarCodeService = new BarCodeService(userId);

                BarCodeDetail newBarCodeDetail = BarCodeService.GetBarCodeByBarCode(model.BarCodeNumber);
                if (newBarCodeDetail == null)
                {
                    ModelState.AddModelError("", $"'{model.BarCodeNumber}' is not a valid Bar Code Number.  Please re-enter or contact a member of the Food Pantry team.");

                    return View(model);
                };

                // Verify/Update Customer
                var custService = new CustomerService(userId);

                CustDetail custDetail = custService.GetCustByBarCodeId(newBarCodeDetail.BarCodeId);
                if (custDetail.CustomerId == 0)
                {
                    ModelState.AddModelError("", $"'{model.BarCodeNumber}' is not assigned to a Customer.  Please re-enter or contact a member of the Food Pantry team.");

                    return View(model);
                }
                return RedirectToAction(actionName: "Edit", controllerName: "Customer", routeValues: new { isCust = model.IsCust, isOrder = true, id = custDetail.CustomerId });
            }
            else
            {
                // Create Customer
                return RedirectToAction(actionName: "Create", controllerName: "Customer", routeValues: new { isOrder = true, barCodeId = 0 });
            }
        }

        // Get: Order/Create
        public ActionResult Create(int custId, bool isCust)
        {
            var userId = User.Identity.GetUserId();
            var custService = new CustomerService(userId);
            var orderService = CreateOrderService();

            CustDetail custDetail = custService.GetCustById(custId);

            OrderCreate model = new OrderCreate
            {
                CustName = $"{custDetail.FirstName} {custDetail.LastName}",
                CustId = custId,
                IsCust = isCust,
                OrderDetailCategoryList = orderService.GetOrderDetailByOrderId(0, userId)
            };

            ViewBag.SlotId = BuildTimeSlotDropdown(false);

            return View(model);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateOrderService();

            if (service.CreateOrder(model))
            {
                TempData["SaveResult"] = $"Order created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"Order could not be created.");

            ViewBag.TimeSlotId = BuildTimeSlotDropdown(model.Deliver);

            return View(model);
        }
        // OrderCreate Stream - Ends here

        // GET: Order/Details
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            var model = orderService.GetOrderWithDetailById(id, userId);

            return View(model);
        }

        //// GET: Order/Update
        //public ActionResult Edit(int id)
        //{
        //    var service = CreateCustomerService();
        //    var detail = service.GetCustById(id);
        //    var model =
        //        new CustUpdate
        //        {
        //            CustomerId = detail.CustomerId,
        //            BarCodeId = detail.BarCodeId,
        //            FirstName = detail.FirstName,
        //            LastName = detail.LastName,
        //            Address = detail.Address,
        //            City = detail.City,
        //            State = detail.State,
        //            ZipCode = detail.ZipCode,
        //            Phone = detail.Phone,
        //            Email = detail.Email,
        //            NumberKids = detail.NumberKids
        //        };

        //    ViewBag.BarCodeId = BuildBarCodeDropdown();

        //    return View(model);
        //}

        //// GET: Order/Update
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, CustUpdate model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    if (model.CustomerId != id)
        //    {
        //        ModelState.AddModelError("", "Id Mismatch");
        //        return View(model);
        //    }

        //    var service = CreateCustomerService();

        //    CustDetail existingCustDetail = service.GetCustByBarCodeId(model.BarCodeId);
        //    if (existingCustDetail != null && existingCustDetail.BarCodeId != id)
        //    {
        //        ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

        //        ViewBag.BarCodeId = BuildBarCodeDropdown();

        //        return View(model);
        //    }
        //    if (service.UpdateCust(model))
        //    {
        //        TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was updated.";
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be updated.");

        //    ViewBag.BarCodeId = BuildBarCodeDropdown();

        //    return View(model);
        //}

        //// GET: Order/Delete
        //public ActionResult Delete(int id)
        //{
        //    var svc = CreateCustomerService();
        //    var model = svc.GetCustById(id);

        //    return View(model);
        //}

        //// GET: Order/Delete
        //[HttpPost]
        //[ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeletePost(int id)
        //{
        //    var service = CreateCustomerService();

        //    service.DeleteCust(id);

        //    TempData["SaveResult"] = $"Customer was deleted";

        //    return RedirectToAction("Index");
        //}

        // Create Order Service
        private OrderService CreateOrderService()
        {
            var userId = User.Identity.GetUserId();
            var service = new OrderService(userId);
            return service;
        }

        // Build TimeSlot Dropdown
        private IOrderedEnumerable<SelectListItem> BuildTimeSlotDropdown(bool deliver)
        {
            var userId = User.Identity.GetUserId();
            var timeSlotService = new TimeSlotService(userId);

            var timeSlotList = new SelectList(timeSlotService.GetTimeSlotDropDown(deliver), "SlotId", "SlotTime");
            var sortedTimeSlotList = timeSlotList.OrderBy(o => o.Text);

            return sortedTimeSlotList;
        }
    }
}