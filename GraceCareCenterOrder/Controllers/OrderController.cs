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
            if (!ModelState.IsValid)
            {
                ViewBag.TimeSlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);
                return View(model);
            }

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
        public ActionResult Details(int id, bool isFromPull, bool isPull)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            bool isCust = false;
            var model = orderService.GetOrderWithShortDetailById(id, userId, isCust);
            model.IsPull = isPull;
            model.IsFromPull = isFromPull;
          
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
            if (!ModelState.IsValid)
            {
                ViewBag.SlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);
                return View(model);
            }

            if (model.OrderId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                ViewBag.SlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);
                return View(model);
            }

            string errorFound = EditItems(model.OrderDetailCategoryList);
            if (errorFound != null)
            {
                ModelState.AddModelError("", errorFound);
                ViewBag.SlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);
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
            ViewBag.SlotId = BuildTimeSlotDropdown(model.Deliver, model.SlotId);
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

        // GET: Order/StartPull
        public ActionResult StartPull(int orderId)
        {
            var service = CreateOrderService();

            bool isCust = false;
            var orderToComplete = service.GetOrderById(orderId, isCust);
            if (orderToComplete.OrderCompletedAt != null)
            {
                TempData["SaveResult"] = $"Order was completed at {orderToComplete.OrderCompletedAt}.";
                return RedirectToAction("PullIndex");
            }

            var userId = User.Identity.GetUserId();
            if (orderToComplete.PullStartedAt != null && orderToComplete.PullStartedBy != userId)
            {
                return RedirectToAction(actionName: "TakeOverPull", controllerName: "Order", routeValues: new { OrderId = orderId });
            }

            bool orderPullStarted = service.StartPullOrder(orderId);
            if (orderPullStarted)
            {
                return RedirectToAction(actionName: "PullOrder", controllerName: "Order", routeValues: new { id = orderId });
            }

            ModelState.AddModelError("", "Order Pull could not be started - unknown reason.");

            return RedirectToAction("PullIndex");
        }

        // GET: Order/TakeOverPull
        public ActionResult TakeOverPull(int orderId)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            bool isCust = false;
            var model = orderService.GetOrderById(orderId, isCust);

            return View(model);
        }

        // GET: Order/TakeOverPull
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TakeOverPull(int orderId, OrderHeaderDetail model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (model.OrderId != orderId)
            {
                ModelState.AddModelError("", "Error in changing Pull User Name.  Please try again.");
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var service = CreateOrderService();

            bool takeOverSucess = service.TakeOverOrder(orderId, userId);
            if (takeOverSucess)
            {
                return RedirectToAction(actionName: "PullOrder", controllerName: "Order", routeValues: new { id = orderId });
            }

            ModelState.AddModelError("", "Error in changing Pull User Name - unknown error.  Please try again.");
            return View(model);
        }


        // GET: Order/PullOrder
        public ActionResult PullOrder(int id)
        {
            var userId = User.Identity.GetUserId();

            var orderService = CreateOrderService();
            bool isCust = false;
            var model = orderService.GetOrderWithShortDetailById(id, userId, isCust);

            return View(model);
        }

        // GET: Order/PullOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PullOrder(int id, OrderHeaderDetail model)
        {
            bool isComplete = false;
            if (Request.Form["Complete"] != null) { isComplete = true; }
            else if (Request.Form["PullOnly"] != null) { isComplete = false; }
            else
            {
                ModelState.AddModelError("", "Programmer Error - Could not identify Button selected.");
                return RedirectToAction("PullIndex");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.OrderId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateOrderService();

            OrderCrtUpdRtnStatus orderRtnStatus = service.UpdatePulledItems(model, id, isComplete);
            if (orderRtnStatus.OrderHeaderCreated)
            {
                if (isComplete)
                {
                    TempData["SaveResult"] = $"Order was Completed.";

                    return RedirectToAction("PullIndex");
                }
                if (orderRtnStatus.OrderAllDetailCreated)
                {
                    TempData["SaveResult"] = $"Order was NOT completed - not all Items were updated. Please review the order.";
                    return RedirectToAction("PullIndex");
                }
                TempData["SaveResult"] = $"Order was NOT completed. Some or all Pulled Items were saved.";
                return RedirectToAction("PullIndex");
            }

            ModelState.AddModelError("", "Order could not be updated. Please review the order.");
            return RedirectToAction("PullIndex");
        }

        // GET: Order/Complete
        public ActionResult Complete(int id)
        {
            var service = CreateOrderService();

            bool isCust = false;
            var orderToComplete = service.GetOrderById(id, isCust);
            if (orderToComplete.OrderCompletedAt != null)
            {
                TempData["SaveResult"] = $"Order was completed at {orderToComplete.OrderCompletedAt}.";
                return RedirectToAction("PullIndex");
            }

            bool orderCompleted = service.CompleteOrder(id);
            if (orderCompleted)
            {
                TempData["SaveResult"] = $"Order for {orderToComplete.CustFirstName} {orderToComplete.CustLastName} was Completed.";
            }
            else
            {
                ModelState.AddModelError("", "Order could not be completed - unknown reason.");
            }

            return RedirectToAction("PullIndex");
        }
        // End - Pull Order section

        // Create Order Service
        private OrderService CreateOrderService()
        {
            var userId = User.Identity.GetUserId();
            var service = new OrderService(userId);
            return service;
        }

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