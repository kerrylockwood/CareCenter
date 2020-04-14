using CareModels.BarCodes;
using CareModels.Customers;
using CareModels.OrderDetails;
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

            bool shortList = false;
            OrderCreate model = new OrderCreate
            {
                CustName = $"{custDetail.FirstName} {custDetail.LastName}",
                CustId = custId,
                IsCust = isCust,
                OrderDetailCategoryList = orderService.GetOrderDetailByOrderId(0, userId, shortList)
            };

            ViewBag.SlotId = BuildTimeSlotDropdown(false, model.SlotId);

            return View(model);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            string errorFound = EditItems(model.OrderDetailCategoryList);
            if (errorFound != null) return View(model);

            var service = CreateOrderService();

            OrderCrtUpdRtnStatus orderRtnStatus = service.CreateOrder(model);
            if (orderRtnStatus.OrderHeaderCreated)
            {
                if (orderRtnStatus.OrderAllDetailCreated)
                {
                    TempData["SaveResult"] = $"Order created - but not all Items were added. Please review the order.";
                    if (model.IsCust)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index");
                }
                TempData["SaveResult"] = $"Order created";
                if (model.IsCust)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Order could not be created.");

            ViewBag.TimeSlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);

            return View(model);
        }
        // OrderCreate Stream - Ends here

        // GET: Order/Details
        public ActionResult Details(int id, bool isPull)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            bool isCust = false;
            var model = orderService.GetOrderWithShortDetailById(id, userId, isCust);
            model.IsPull = isPull;

            return View(model);
        }

        // GET: Order/Update
        public ActionResult Edit(int id, bool isCust)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            var model = orderService.GetOrderWithDetailById(id, userId, isCust);

            ViewBag.SlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);

            return View(model);
        }

        // GET: Order/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderUpdate model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.OrderId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            string errorFound = EditItems(model.OrderDetailCategoryList);
            if (errorFound != null)
            {
                ModelState.AddModelError("", errorFound);
                return View(model);
            }

            var service = CreateOrderService();

            OrderCrtUpdRtnStatus orderRtnStatus = service.UpdateOrder(model);
            if (orderRtnStatus.OrderHeaderCreated)
            {
                if (orderRtnStatus.OrderAllDetailCreated)
                {
                    TempData["SaveResult"] = $"Order was updated.";
                    if (model.IsCust)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index");
                }
                TempData["SaveResult"] = $"Order was updated - not all Items were updated. Please review the order.";
                if (model.IsCust)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Order could not be updated.");

            return View(model);
        }

        // GET: Order/Delete
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            bool isCust = false;
            var model = orderService.GetOrderWithShortDetailById(id, userId, isCust);

            return View(model);
        }

        // GET: Order/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateOrderService();

            service.DeleteOrder(id);

            TempData["SaveResult"] = $"Order was deleted";

            return RedirectToAction("Index");
        }

        // Create Order Service
        private OrderService CreateOrderService()
        {
            var userId = User.Identity.GetUserId();
            var service = new OrderService(userId);
            return service;
        }

        // Edit Items in an Order
        private string EditItems(List<OrderDetailCategory> orderDetailCategoryList)
        {
            double pointsUsed = 0;
            int maxPointsAllowed = 25;
            foreach (var cat in orderDetailCategoryList)
            {
                foreach (var subCat in cat.OrderDetailSubCatList)
                {
                    int subCatCount = 0;
                    int subCatMax = (subCat.SubCatMaxAllowed == 0) ? 999999999 : subCat.SubCatMaxAllowed;
                    foreach (var itm in subCat.OrderDetailItemList)
                    {
                        if (itm.MaxAllowed > 0)
                        {
                            if (itm.Quantity > itm.MaxAllowed)
                            {
                                return $"Quantity selected for {itm.ItemName} is more than the maximum allowed of {itm.MaxAllowed}";
                            }
                        }
                        subCatCount += itm.Quantity;

                        if (subCatCount > subCatMax)
                        {
                            return $"Total quantity for {subCat.SubCatName} is more than the maximum allowed of {subCatMax}";
                        }

                        pointsUsed += itm.Quantity * itm.PointCost;
                    }
                }
            }
            if (pointsUsed > maxPointsAllowed)
            {
                return $"{pointsUsed} points worth of items were selected, but only {maxPointsAllowed} points are available.  Please reduce some quantities.";
            }

            return null;
        }

        // Begin - Pull Order section
        // GET: OrderPull
        public ActionResult PullIndex()
        {
            var userId = User.Identity.GetUserId();
            var service = CreateOrderService();
            var model = service.GetUnpulledOrders();

            // Get the Appointment Date/Time for each Order
            foreach (var item in model)
            {
                item.SlotDateTime = service.ConvertSlotToDateTime(item.SlotId, item.CreateDateTime.DateTime, item.Deliver, userId);
            }

            return View(model);
        }
        // End - Pull Order section

        // Build TimeSlot Dropdown
        private IOrderedEnumerable<SelectListItem> BuildTimeSlotDropdown(bool deliver, int selectedValue)
        {
            var userId = User.Identity.GetUserId();
            var timeSlotService = new TimeSlotService(userId);

            var timeSlotList = new SelectList(timeSlotService.GetTimeSlotDropDown(deliver), "SlotId", "SlotTime", selectedValue);
            var sortedTimeSlotList = timeSlotList.OrderBy(o => o.Text);

            return sortedTimeSlotList;
        }
    }
}