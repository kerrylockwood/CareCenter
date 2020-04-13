using CareData;
using CareModels.OrderDetails;
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

        public OrderHeaderDetail GetOrderHeaderByCustId(int id, bool isCust)
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
                        .SingleOrDefault(e => e.CustId == id);
                return
                new OrderHeaderDetail
                {
                    OrderId = entity.OrderId,
                    CustId = entity.CustId,
                    SlotId = entity.TimeSlot.SlotId,
                    IsCust = isCust,
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

        public OrderHeaderDetail GetOrderById(int id, bool isCust)
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
                    IsCust = isCust,
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

        public OrderUpdate GetOrderUpdateById(int id, bool isCust)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .OrderHeaders
                        .Single(e => e.OrderId == id);
                return
                new OrderUpdate
                {
                    OrderId = entity.OrderId,
                    CustId = entity.CustId,
                    SlotId = entity.TimeSlot.SlotId,
                    IsCust = isCust,
                    SlotDateTime = new DateTime(),
                    CustName = $"{entity.Customer.FirstName} {entity.Customer.LastName}",
                    //CustFirstName = entity.Customer.FirstName,
                    //CustLastName = entity.Customer.LastName,
                    Deliver = entity.Deliver,
                    //PullStartedAt = entity.PullStartedAt,
                    //PullStartedBy = entity.PullStartedBy,
                    //PullStartedName = (entity.PullStartedBy == null) ? null : entity.PullStartedUser.UserName,
                    //OrderCompletedAt = entity.OrderCompletedAt,
                    //PullCompleted = (entity.OrderCompletedAt == null) ? false : true,
                    MostWantedNotes = entity.MostWantedNotes,
                    FreezerNotes = entity.FreezerNotes,
                    ProduceNotes = entity.ProduceNotes,
                    NonFoodNotes = entity.NonFoodNotes,
                    CreateDateTime = entity.CreatedAt,
                    //CreateName = entity.User.UserName,
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

        //public OrderHeaderDetail GetOrderWithDetailById(int id, string userId, bool isCust)
        public OrderUpdate GetOrderWithDetailById(int id, string userId, bool isCust)
        {
            //OrderHeaderDetail model = GetOrderById(id, isCust);
            OrderUpdate model = GetOrderUpdateById(id, isCust);

            model.SlotDateTime = ConvertSlotToDateTime(model.SlotId, model.CreateDateTime.DateTime, model.Deliver, userId);
            bool shortList = false;
            model.OrderDetailCategoryList = GetOrderDetailByOrderId(id, userId, shortList);

            return model;
        }

        public OrderHeaderDetail GetOrderWithShortDetailById(int id, string userId, bool isCust)
        {
            OrderHeaderDetail model = GetOrderById(id, isCust);

            model.SlotDateTime = ConvertSlotToDateTime(model.SlotId, model.CreateDateTime.DateTime, model.Deliver, userId);
            bool shortList = true;
            model.OrderDetailCategoryList = GetOrderDetailByOrderId(id, userId, shortList);

            return model;
        }

        public List<OrderDetailCategory> GetOrderDetailByOrderId(int id, string userId, bool shortList)
        {
            List<OrderDetailCategory> catDtlList = new List<OrderDetailCategory>();

            var catService = new CategoryService(userId);

            var newCategoryList = catService.GetCategories().OrderBy(o => o.CategoryName);
            foreach (CareModels.Catagories.CategoryList cat in newCategoryList)
            {
                List<OrderDetailSubCat> subCatDtlList = new List<OrderDetailSubCat>();
                var subCatService = new SubCatService(userId);

                bool subCatAdded = false;
                var newSubCatList = subCatService.GetSubCatsByCatId(cat.CategoryId);
                foreach (CareModels.SubCategories.SubCatListShort subCat in newSubCatList)
                {
                    List<OrderDetailItem> itemDtl = new List<OrderDetailItem>();
                    var itemService = new ItemService(userId);

                    var newItemList = itemService.GetItemsBySubCatId(subCat.SubCatId);
                    bool itemAdded = false;
                    foreach (CareModels.Items.ItemListShort itm in newItemList)
                    {
                        //OrderDetailItem snglItemDtl = new OrderDetailItem();
                        var orderDetailService = new OrderDetailService(userId);

                        var newItemDtl = orderDetailService.GetOrderDetailByOrderIdAndItemId(id, itm.ItemId);

                        if ((shortList && newItemDtl.ItemId > 0) || !shortList)
                        {
                            OrderDetailItem itmDtl = new OrderDetailItem
                            {
                                OrderDetailId = newItemDtl.OrderDetailId,
                                ItemId = itm.ItemId,
                                ItemName = itm.ItemName,
                                AisleNumber = itm.AisleNumber,
                                MaxAllowed = itm.MaxAllowed,
                                PointCost = itm.PointCost,
                                Quantity = newItemDtl.Quantity,
                                QuantityBefore = newItemDtl.Quantity
                            };
                            itemDtl.Add(itmDtl);
                            itemAdded = true;
                        }
                    }
                    if (itemAdded)
                    {
                        OrderDetailSubCat dtlSubCat = new OrderDetailSubCat
                        {
                            SubCatId = subCat.SubCatId,
                            CategoryId = cat.CategoryId,
                            SubCatName = subCat.SubCatName,
                            SubCatMaxAllowed = subCat.SubCatMaxAllowed,
                            OrderDetailItemList = itemDtl
                        };
                        subCatDtlList.Add(dtlSubCat);
                        subCatAdded = true;
                    }
                }
                if (subCatAdded)
                {
                    OrderDetailCategory dtlCat = new OrderDetailCategory
                    {
                        CategoryId = cat.CategoryId,
                        CategoryName = cat.CategoryName,
                        OrderDetailSubCatList = subCatDtlList
                    };
                    catDtlList.Add(dtlCat);
                }
            }

            return catDtlList;
        }

        public OrderCrtUpdRtnStatus CreateOrder(OrderCreate model)
        {
            OrderCrtUpdRtnStatus orderRtnStatus = new OrderCrtUpdRtnStatus
            {
                OrderHeaderCreated = false,
                OrderAllDetailCreated = false,
                OrderId = 0
            };
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

                try { ctx.SaveChanges(); }
                catch { return orderRtnStatus; }

                orderRtnStatus.OrderHeaderCreated = true;
                orderRtnStatus.OrderId = entity.OrderId;

                // Assume all OrderDetail records will be written - make false if any fail.
                orderRtnStatus.OrderAllDetailCreated = true;
                OrderDetailService orderDetailService = new OrderDetailService(_userId);

                // Add Order Detail
                foreach (var catagory in model.OrderDetailCategoryList)
                {
                    foreach (var subCat in catagory.OrderDetailSubCatList)
                    {
                        if (subCat.OrderDetailItemList != null)
                        {
                            foreach (var itm in subCat.OrderDetailItemList)
                            {
                                if (itm.Quantity > 0)
                                {
                                    var orderDetail =
                                        new OrderDetailCreate()
                                        {
                                            OrderId = orderRtnStatus.OrderId,
                                            ItemId = itm.ItemId,
                                            Quantity = itm.Quantity,
                                            QuantityBefore = itm.Quantity,
                                            Filled = false
                                        };

                                    if (!orderDetailService.CreateOrderDetail(orderDetail))
                                    {
                                        orderRtnStatus.OrderAllDetailCreated = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return orderRtnStatus;
        }

        public OrderCrtUpdRtnStatus UpdateOrder(OrderUpdate model)
        {
            OrderCrtUpdRtnStatus orderRtnStatus = new OrderCrtUpdRtnStatus
            {
                OrderHeaderCreated = false,
                OrderAllDetailCreated = false,
                OrderId = 0
            };
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .OrderHeaders
                    .Single(e => e.OrderId == model.OrderId);
                entity.CustId = model.CustId;
                entity.SlotId = model.SlotId;
                entity.MostWantedNotes = model.MostWantedNotes;
                entity.FreezerNotes = model.FreezerNotes;
                entity.ProduceNotes = model.ProduceNotes;
                entity.NonFoodNotes = model.NonFoodNotes;
                entity.Deliver = model.Deliver;
                entity.PullStartedAt = null;
                entity.PullStartedBy = null;
                entity.OrderCompletedAt = null;

                try { ctx.SaveChanges(); }
                catch { return orderRtnStatus; }
            }
            orderRtnStatus.OrderHeaderCreated = true;

            orderRtnStatus.OrderId = model.OrderId;

            // Assume all OrderDetail records will be created/updated/deleted - make false if any fail.
            orderRtnStatus.OrderAllDetailCreated = true;
            OrderDetailService orderDetailService = new OrderDetailService(_userId);

            // Add Order Detail
            foreach (var catagory in model.OrderDetailCategoryList)
            {
                foreach (var subCat in catagory.OrderDetailSubCatList)
                {
                    if (subCat.OrderDetailItemList != null)
                    {
                        foreach (var itm in subCat.OrderDetailItemList)
                        {
                            if (itm.Quantity != itm.QuantityBefore)
                            {
                                if (itm.Quantity > 0 && itm.QuantityBefore > 0)
                                {
                                    // Quantity changed
                                    var orderDetail =
                                        new OrderDetailUpdate()
                                        {
                                            OrderDetailId = itm.OrderDetailId,
                                            ItemId = itm.ItemId,
                                            Quantity = itm.Quantity,
                                            Filled = false
                                        };

                                    if (!orderDetailService.UpdateOrderDetail(orderDetail))
                                    {
                                        orderRtnStatus.OrderAllDetailCreated = false;
                                    }
                                }
                                else if (itm.Quantity > 0)
                                {
                                    // Add Detail
                                    var orderDetail =
                                        new OrderDetailCreate()
                                        {
                                            OrderId = orderRtnStatus.OrderId,
                                            ItemId = itm.ItemId,
                                            Quantity = itm.Quantity,
                                            Filled = false
                                        };

                                    if (!orderDetailService.CreateOrderDetail(orderDetail))
                                    {
                                        orderRtnStatus.OrderAllDetailCreated = false;
                                    }
                                }
                                else if (itm.QuantityBefore > 0)
                                {
                                    // Delete Detail
                                    //var orderDetail =
                                    //    new OrderDetailCreate()
                                    //    {
                                    //        OrderId = orderRtnStatus.OrderId,
                                    //        ItemId = itm.ItemId,
                                    //        Quantity = itm.Quantity,
                                    //        Filled = false
                                    //    };

                                    if (!orderDetailService.DeleteOrderDetail(itm.OrderDetailId))
                                    {
                                        orderRtnStatus.OrderAllDetailCreated = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return orderRtnStatus;
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

        public bool DeleteOrder(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .OrderHeaders
                        .Single(e => e.OrderId == id);

                ctx.OrderHeaders.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

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
