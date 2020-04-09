using CareData;
using CareModels.Orders;
using CareModels.TimeSlots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class OrderService
    {
        private readonly string _userId;

        public object User { get; private set; }

        public OrderService(string userId)
        {
            _userId = userId;
        }

        public IEnumerable<OrderList> GetOrders()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .OrderHeaders
                        .Select(
                            e =>
                                new OrderList
                                {
                                    OrderId = e.OrderId,
                                    CustId = e.CustId,
                                    SlotId = e.SlotId,
                                    CreateDateTime = e.CreatedAt,
                                    //SlotDateTime cannot be populated here due to LINQ
                                    CustFirstName = e.Customer.FirstName,
                                    CustLastName = e.Customer.LastName,
                                    Deliver = e.Deliver,
                                    PullStarted = (e.PullStartedAt == null) ? false : true,
                                    PullCompleted = (e.OrderCompletedAt == null) ? false : true
                                }
                        );

                return query.ToArray();
            }
        }

        public OrderHeaderDetail GetOrderHeaderByCustId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.OrderHeaders.Count(e => e.CustId == id) == 0)
                {
                    return new OrderHeaderDetail();
                }
                var entity =
                    ctx
                        .OrderHeaders
                        .Single(e => e.OrderId == id);
                return
                new OrderHeaderDetail
                {
                    OrderId = entity.OrderId,
                    CustId = entity.CustId,
                    SlotId = entity.TimeSlot.SlotId,
                    SlotDateTime = new DateTime(),
                    CustFirstName = entity.Customer.FirstName,
                    CustLastName = entity.Customer.LastName,
                    Deliver = entity.Deliver,
                    PullStartedAt = entity.PullStartedAt,
                    PullStartedBy = entity.PullStartedBy,
                    PullStartedName = (entity.PullStartedBy == null) ? null : entity.PullStartedUser.UserName,
                    OrderCompletedAt = entity.OrderCompletedAt,
                    PullCompleted = (entity.OrderCompletedAt == null) ? false : true,
                    MostWantedNotes = entity.MostWantedNotes,
                    FreezerNotes = entity.FreezerNotes,
                    ProduceNotes = entity.ProduceNotes,
                    NonFoodNotes = entity.NonFoodNotes,
                    CreateDateTime = entity.CreatedAt,
                    CreateName = entity.User.UserName,
                    OrderDetailCategoryList = new List<OrderDetailCategory>()
                };
            }
        }

        public OrderHeaderDetail GetOrderById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .OrderHeaders
                        .Single(e => e.OrderId == id);
                return
                new OrderHeaderDetail
                {
                    OrderId = entity.OrderId,
                    CustId = entity.CustId,
                    SlotId = entity.TimeSlot.SlotId,
                    SlotDateTime = new DateTime(),
                    CustFirstName = entity.Customer.FirstName,
                    CustLastName = entity.Customer.LastName,
                    Deliver = entity.Deliver,
                    PullStartedAt = entity.PullStartedAt,
                    PullStartedBy = entity.PullStartedBy,
                    PullStartedName = (entity.PullStartedBy == null) ? null : entity.PullStartedUser.UserName,
                    OrderCompletedAt = entity.OrderCompletedAt,
                    PullCompleted = (entity.OrderCompletedAt == null) ? false : true,
                    MostWantedNotes = entity.MostWantedNotes,
                    FreezerNotes = entity.FreezerNotes,
                    ProduceNotes = entity.ProduceNotes,
                    NonFoodNotes = entity.NonFoodNotes,
                    CreateDateTime = entity.CreatedAt,
                    CreateName = entity.User.UserName,
                    OrderDetailCategoryList = new List<OrderDetailCategory>()
                };
            }
        }

        public int GetSlotCount(int slotId, bool delivery)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var timeSlotService = new TimeSlotService(_userId);

                var lastTimeSlot = timeSlotService.GetMaxTimeSlot();
                DateTimeOffset excludeCreateBefore = ConvertSlotToDateTime(lastTimeSlot.SlotId, DateTime.Now, delivery, _userId);

                int cnt = ctx.OrderHeaders.Count(e => e.SlotId == slotId && e.Deliver == delivery && e.CreatedAt > excludeCreateBefore);
                {
                    return cnt;
                }
            }
        }

        public OrderHeaderDetail GetOrderWithDetailById(int id, string userId)
        {
            //List<OrderDetailCategory> catDtlList = new List<OrderDetailCategory>();

            //var catService = new CategoryService(userId);

            //var newCategoryList = catService.GetCategories().OrderBy(o => o.CategoryName);
            //foreach (CareModels.Catagories.CategoryList cat in newCategoryList)
            //{
            //    List<OrderDetailSubCat> subCatDtlList = new List<OrderDetailSubCat>();
            //    var subCatService = new SubCatService(userId);

            //    var newSubCatList = subCatService.GetSubCatsByCatId(cat.CategoryId);
            //    foreach (CareModels.SubCategories.SubCatListShort subCat in newSubCatList)
            //    {
            //        List<OrderDetailItem> itemDtl = new List<OrderDetailItem>();
            //        var itemService = new ItemService(userId);

            //        var newItemList = itemService.GetItemsBySubCatId(subCat.SubCatId);
            //        foreach (CareModels.Items.ItemListShort itm in newItemList)
            //        {
            //            //OrderDetailItem snglItemDtl = new OrderDetailItem();
            //            var orderDetailService = new OrderDetailService(userId);

            //            var newItemDtl = orderDetailService.GetOrderDetailByOrderIdAndItemId(id, itm.ItemId);

            //            OrderDetailItem itmDtl = new OrderDetailItem
            //            {
            //                ItemId = itm.ItemId,
            //                ItemName = itm.ItemName,
            //                AisleNumber = itm.AisleNumber,
            //                MaxAllowed = itm.MaxAllowed,
            //                PointCost = itm.PointCost,
            //                Quantity = newItemDtl.Quantity
            //            };
            //            itemDtl.Add(itmDtl);
            //        }
            //        OrderDetailSubCat dtlSubCat = new OrderDetailSubCat
            //        {
            //            SubCatId = subCat.SubCatId,
            //            CategoryId = cat.CategoryId,
            //            SubCatName = subCat.SubCatName,
            //            SubCatMaxAllowed = subCat.SubCatMaxAllowed,
            //            OrderDetailItemList = itemDtl
            //        };
            //        subCatDtlList.Add(dtlSubCat);
            //    }

            //    OrderDetailCategory dtlCat = new OrderDetailCategory
            //    {
            //        CategoryId = cat.CategoryId,
            //        CategoryName = cat.CategoryName,
            //        OrderDetailSubCatList = subCatDtlList
            //    };
            //    catDtlList.Add(dtlCat);
            //}

            OrderHeaderDetail model = GetOrderById(id);

            model.SlotDateTime = ConvertSlotToDateTime(model.SlotId, model.CreateDateTime.DateTime, model.Deliver, userId);
            model.OrderDetailCategoryList = GetOrderDetailByOrderId(id, userId);

            return model;
        }

        public List<OrderDetailCategory> GetOrderDetailByOrderId(int id, string userId)
        {
            List<OrderDetailCategory> catDtlList = new List<OrderDetailCategory>();

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

            return catDtlList;
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

        public bool CreateOrder(OrderCreate model)
        {
            var entity =
                new OrderHeader()
                {
                    CustId = model.CustId,
                    SlotId = model.SlotId,
                    MostWantedNotes = model.MostWantedNotes,
                    FreezerNotes = model.FreezerNotes,
                    ProduceNotes = model.ProduceNotes,
                    NonFoodNotes = model.NonFoodNotes,
                    Deliver = model.Deliver,
                    PullStartedAt = null,
                    PullStartedBy = null,
                    OrderCompletedAt = null,
                    CreateBy = _userId,
                    CreatedAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.OrderHeaders.Add(entity);

                //loop here to add Order Detail

                return ctx.SaveChanges() == 1;
            }
        }

        //public bool UpdateItem(ItemUpdate model)
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        var entity =
        //            ctx
        //                .Items
        //                .Single(e => e.ItemId == model.ItemId);
        //        entity.SubCatId = model.SubCatId;
        //        entity.ItemName = model.ItemName;
        //        entity.AisleNumber = model.AisleNumber;
        //        entity.MaxAllowed = model.MaxAllowed;
        //        entity.PointCost = model.PointCost;

        //        return ctx.SaveChanges() == 1;
        //    }
        //}

        //public bool DeleteItem(int id)
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        var entity =
        //            ctx
        //                .Items
        //                .Single(e => e.ItemId == id);

        //        ctx.Items.Remove(entity);

        //        return ctx.SaveChanges() == 1;
        //    }
        //}

        //// Get Slot Date/Time from Slot DayOfWeek
        //public DateTime ConvertSlotToDateTime(int slotId, DateTime createDateTime)
        //{
        //    var userId = User.Identity.GetUserId();
        //    var timeSlotService = new TimeSlotService(userId);

        //    var slotDetail = timeSlotService.GetTimeSlotById(slotId);
        //    //SlotDetail slotDetail = GetTimeSlotById(slotId);
        //    TimeSpan Time = new TimeSpan(19, 00, 00);
        //    int DayOfWeek = 7;
        //    //*******get slot info here

        //    DateTime weekStartDate = GetWeekStartDate(createDateTime);

        //    return weekStartDate.AddDays(DayOfWeek).Add(Time);
        //}

        //// Get Slot Date/Time from Slot DayOfWeek
        //public DateTime GetWeekStartDate(DateTime createDateTime)
        //{
        //    // Assumption: Customers can start creating orders on the
        //    //    day after the last Pickup Slot day and Pickups start
        //    //    as early as Sunday
        //    // Get Sunday Date based on date passed in (should be either
        //    //    Create Date or current date if creating now)
        //    //*******get *last* slot here
        //    TimeSpan lastSlotTime = new TimeSpan(19, 00, 00);
        //    int lastSlotDayOfWeek = 7;
        //    //*******get *last* slot here

        //    int createDayOfWeek = (int)createDateTime.DayOfWeek;
        //    DateTime weekStartDate
        //        = createDateTime.Date.AddDays(createDayOfWeek * -1);
        //    DateTime orderCutoffTime = weekStartDate.AddDays(lastSlotDayOfWeek).Add(lastSlotTime).AddMinutes(-60);

        //    if (createDateTime > orderCutoffTime)
        //    {
        //        // Created after last appointment slot (less buffer)
        //        // Need to set Start Date to following Sunday
        //        weekStartDate = weekStartDate.AddDays(7);
        //    }

        //    return weekStartDate;
        //}

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime ConvertSlotToDateTime(int slotId, DateTime createDateTime, bool delivery, string userId)
        {
            var timeSlotService = new TimeSlotService(userId);

            var slotDetail = timeSlotService.GetTimeSlotById(slotId);

            DateTime weekStartDate = GetWeekStartDate(createDateTime, delivery, userId);

            return weekStartDate.AddDays(slotDetail.DayOfWeekNum).Add(slotDetail.Time);
        }

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime GetWeekStartDate(DateTime createDateTime, bool delivery, string userId)
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
    }
}
