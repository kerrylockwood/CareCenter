using CareData;
using CareModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class OrderDetailService
    {
        private readonly string _userId;

        public object User { get; private set; }

        public OrderDetailService(string userId)
        {
            _userId = userId;
        }

        //public IEnumerable<OrderList> GetOrders()
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        var query =
        //            ctx
        //                .OrderHeaders
        //                .Select(
        //                    e =>
        //                        new OrderList
        //                        {
        //                            OrderId = e.OrderId,
        //                            CustId = e.CustId,
        //                            SlotId = e.SlotId,
        //                            CreateDateTime = e.CreatedAt,
        //                            //SlotDateTime cannot be populated here due to LINQ
        //                            CustFirstName = e.Customer.FirstName,
        //                            CustLastName = e.Customer.LastName,
        //                            Deliver = e.Deliver,
        //                            PullStarted = (e.PullStartedAt == null) ? false : true,
        //                            PullCompleted = (e.OrderCompletedAt == null) ? false : true
        //                        }
        //                );

        //        return query.ToArray();
        //    }
        //}

        public OrderDetailItem GetOrderDetailByOrderIdAndItemId(int orderId, int itemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.OrderDetails.Count(e => e.OrderId == orderId && e.ItemId == itemId)
                    == 0)
                {
                    return
                    new OrderDetailItem
                    {
                        ItemId = 0,
                        SubCatId = 0,
                        ItemName = null,
                        AisleNumber = 0,
                        MaxAllowed = 0,
                        PointCost = 0.0,
                        Quantity = 0
                    };
                }
                var entity =
                    ctx
                        .OrderDetails
                        .Single(e => e.OrderId == orderId && e.ItemId == itemId);
                return
                    new OrderDetailItem
                    {
                        ItemId = entity.ItemId,
                        SubCatId = entity.Item.SubCatId,
                        ItemName = entity.Item.ItemName,
                        AisleNumber = entity.Item.AisleNumber,
                        MaxAllowed = entity.Item.MaxAllowed,
                        PointCost = entity.Item.PointCost,
                        Quantity = entity.Quantity
                    };
            }
        }

        //public bool CreateItem(ItemCreate model)
        //{
        //    var entity =
        //        new Item()
        //        {
        //            //ItemId = model.ItemId,
        //            SubCatId = model.SubCatId,
        //            ItemName = model.ItemName,
        //            AisleNumber = model.AisleNumber,
        //            MaxAllowed = model.MaxAllowed,
        //            PointCost = model.PointCost,
        //            CreateBy = _userId,
        //            CreateAt = DateTimeOffset.Now
        //        };
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        ctx.Items.Add(entity);
        //        return ctx.SaveChanges() == 1;
        //    }
        //}

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
    }
}
