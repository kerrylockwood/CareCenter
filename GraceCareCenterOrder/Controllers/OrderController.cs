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
            var service = CreateOrderService();
            var model = service.GetOrders();

            // Get the Appointment Date/Time for each Order
            foreach (var item in model)
            {
                item.SlotDateTime = ConvertSlotToDateTime(item.SlotId, item.CreateDateTime.DateTime, item.Deliver);
            }

            return View(model);
        }

        ////Move to OrderCreate - begin
        //// GET: Customer/BarCode
        //public ActionResult BarCodeDetails()
        //{
        //    return View();
        //}

        //// POST: Customer/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult BarCodeValidate(CustBarCode model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var service = CreateCustomerService();

        //    if (service.ValidateCustBarCode(model.BarCodeNumber) == null && model.BarCodeNumber > 0)
        //    {
        //        ModelState.AddModelError("", $"'{model.BarCodeNumber}' is not a valid Bar Code Number.  Please re-enter or contact a member of the Food Pantry team.");

        //        return View(model);
        //    };

        //    TempData["BarCodeId"] = model.BarCodeId;
        //    TempData["BarCodeNumber"] = model.BarCodeNumber;
        //    return RedirectToAction("Create");
        //}
        ////Move to OrderCreate - end

        //// Get: Customer/Create
        //public ActionResult Create()
        //{
        //    ViewBag.BarCodeId = BuildBarCodeDropdown();

        //    return View();
        //}

        //// POST: Customer/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CustCreate model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var service = CreateCustomerService();

        //    CustDetail existingCustDetail = service.GetCustByBarCodeId(model.BarCodeId);
        //    if (existingCustDetail != null)
        //    {
        //        ModelState.AddModelError("", $"BarCode '{existingCustDetail.BarCodeNumber}' is already assigned to {existingCustDetail.FirstName} {existingCustDetail.LastName}.");

        //        ViewBag.BarCodeId = BuildBarCodeDropdown();

        //        return View(model);
        //    }
        //    if (service.CreateCust(model))
        //    {
        //        TempData["SaveResult"] = $"'{model.FirstName} {model.LastName}' was created";
        //        return RedirectToAction("Index");
        //    };

        //    ModelState.AddModelError("", $"'{model.FirstName} {model.LastName}' could not be created.");

        //    ViewBag.BarCodeId = BuildBarCodeDropdown();

        //    return View(model);
        //}

        // GET: Customer/Details
        public ActionResult Details(int id)
        {
            List<OrderDetailCategory> catDtlList = new List<OrderDetailCategory>();

            var userId = User.Identity.GetUserId();
            var catService = new CategoryService(userId);

            var newCategoryList = catService.GetCategories().OrderBy(o => o.CategoryName);
            foreach (CareModels.Catagories.CategoryList cat in newCategoryList)
            {
                List<OrderDetailSubCat> subCatDtlList = new List<OrderDetailSubCat>();
                var subCatService = new SubCatService(userId);

                var newSubCatList = subCatService.GetSubCatsByCatId(cat.CategoryId);
                foreach (CareModels.SubCategories.SubCatListShort subCat in newSubCatList)
                {
                    List<OrderDetailItem> itemDtl = new List<OrderDetailItem>();
                    var itemService = new ItemService(userId);

                    var newItemList = itemService.GetItemsBySubCatId(subCat.SubCatId);
                    foreach (CareModels.Items.ItemListShort itm in newItemList)
                    {
                        //OrderDetailItem snglItemDtl = new OrderDetailItem();
                        var orderDetailService = new OrderDetailService(userId);

                        var newItemDtl = orderDetailService.GetOrderDetailByOrderIdAndItemId(id, itm.ItemId);

                        OrderDetailItem itmDtl = new OrderDetailItem
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            AisleNumber = itm.AisleNumber,
                            MaxAllowed = itm.MaxAllowed,
                            PointCost = itm.PointCost,
                            Quantity = newItemDtl.Quantity
                        };
                        itemDtl.Add(itmDtl);
                    }
                    OrderDetailSubCat dtlSubCat = new OrderDetailSubCat
                    {
                        SubCatId = subCat.SubCatId,
                        CategoryId = cat.CategoryId,
                        SubCatName = subCat.SubCatName,
                        SubCatMaxAllowed = subCat.SubCatMaxAllowed,
                        OrderDetailItemList = itemDtl
                    };
                    subCatDtlList.Add(dtlSubCat);
                }

                OrderDetailCategory dtlCat = new OrderDetailCategory
                {
                    CategoryId = cat.CategoryId,
                    CategoryName = cat.CategoryName,
                    OrderDetailSubCatList = subCatDtlList
                };
                catDtlList.Add(dtlCat);
            }

            var OrderService = CreateOrderService();
            var model = OrderService.GetOrderById(id);

            model.SlotDateTime = ConvertSlotToDateTime(model.SlotId, model.CreateDateTime.DateTime, model.Deliver);
            model.OrderDetailCategoryList = catDtlList;

            return View(model);
        }

        //// GET: Customer/Update
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

        //// GET: Customer/Update
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

        //// GET: Customer/Delete
        //public ActionResult Delete(int id)
        //{
        //    var svc = CreateCustomerService();
        //    var model = svc.GetCustById(id);

        //    return View(model);
        //}

        //// GET: Customer/Delete
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

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime ConvertSlotToDateTime(int slotId, DateTime createDateTime, bool delivery)
        {
            var userId = User.Identity.GetUserId();
            var timeSlotService = new TimeSlotService(userId);

            var slotDetail = timeSlotService.GetTimeSlotById(slotId);

            DateTime weekStartDate = GetWeekStartDate(createDateTime, delivery);

            return weekStartDate.AddDays(slotDetail.DayOfWeekNum).Add(slotDetail.Time);
        }

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime GetWeekStartDate(DateTime createDateTime, bool delivery)
        {
            // Gets Sunday's Date of the Week where Customers can next pick
            //    up orders.  If the createDateTime is before the last time
            //    slot available for pickup, this date will be the Sunday
            //    before.  Otherwise it will be the Sunday after.
            // createDateTime passed in will be the DateTime the order was
            //    created for existing orders or the current DateTime for
            //    new orders.
            // Assumption: Customers can start creating orders on the
            //    day after the last Pickup Slot day and Pickups start
            //    as early as Sunday

            // Get Last Slot Day and Time. This will be used to determine
            //    if Sunday will be before or after the createDateTime passed in
            var userId = User.Identity.GetUserId();
            var timeSlotService = new TimeSlotService(userId);

            var slotDetail = timeSlotService.GetMaxTimeSlot();
            int lastSlotDayOfWeek = slotDetail.DayOfWeekNum;
            TimeSpan lastSlotTime = slotDetail.Time;

            // To get weekStartDate: 1) strip off time, 2) subtract the
            //    number of days we are into the next week.
            // Assumption (again): First day of week for pickup is Sunday
            int createDayOfWeek = (int)createDateTime.DayOfWeek;
            DateTime weekStartDate
                = createDateTime.Date.AddDays(createDayOfWeek * -1);
            DateTime orderCutoffTime = new DateTime();

            // For Delivery, cutoff time is midnight yesterday.  Otherwise
            //    it is 2 hours (randomly selected) ago.
            if (delivery)
            {
                orderCutoffTime = weekStartDate.AddDays(lastSlotDayOfWeek);
            }
            else
            {
                orderCutoffTime = weekStartDate.AddDays(lastSlotDayOfWeek).Add(lastSlotTime).AddMinutes(-120);
            }

            if (createDateTime >= orderCutoffTime)
            {
                // Created after last appointment slot (less buffer)
                // Need to set Start Date to following Sunday
                weekStartDate = weekStartDate.AddDays(7);
            }

            return weekStartDate;
        }
        //// Build BarCode Dropdown
        //private IOrderedEnumerable<SelectListItem> BuildBarCodeDropdown()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var barCodeService = new BarCodeService(userId);

        //    var barCodeList = new SelectList(barCodeService.GetBarCodes(), "BarCodeId", "BarCodeNumber");
        //    var sortedBarCodeList = barCodeList.OrderBy(o => o.Text);

        //    return sortedBarCodeList;
        //}
    }
}