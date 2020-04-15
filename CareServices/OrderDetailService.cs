using CareData;
using CareModels.OrderDetails;
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

        public OrderDetailItem GetOrderDetailById(int orderDetailId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.OrderDetails.Count(e => e.OrderDetailId == orderDetailId)
                    == 0)
                {
                    return
                    new OrderDetailItem
                    {
                        OrderDetailId = 0,
                        ItemId = 0,
                        SubCatId = 0,
                        ItemName = null,
                        AisleNumber = 0,
                        MaxAllowed = 0,
                        PointCost = 0.0,
                        Quantity = 0,
                        QuantityBefore = 0,
                        Pulled = false,
                        PulledBefore = false
                    };
                }
                var entity =
                    ctx
                        .OrderDetails
                        .Single(e => e.OrderDetailId == orderDetailId);
                return
                    new OrderDetailItem
                    {
                        OrderDetailId = entity.OrderDetailId,
                        ItemId = entity.ItemId,
                        SubCatId = entity.Item.SubCatId,
                        ItemName = entity.Item.ItemName,
                        AisleNumber = entity.Item.AisleNumber,
                        MaxAllowed = entity.Item.MaxAllowed,
                        PointCost = entity.Item.PointCost,
                        Quantity = entity.Quantity,
                        QuantityBefore = entity.Quantity,
                        Pulled = entity.Filled,
                        PulledBefore = entity.Filled
                    };
            }
        }

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
                        OrderDetailId = 0,
                        ItemId = 0,
                        SubCatId = 0,
                        ItemName = null,
                        AisleNumber = 0,
                        MaxAllowed = 0,
                        PointCost = 0.0,
                        Quantity = 0,
                        QuantityBefore = 0,
                        Pulled = false,
                        PulledBefore = false
                    };
                }
                var entity =
                    ctx
                        .OrderDetails
                        .Single(e => e.OrderId == orderId && e.ItemId == itemId);
                return
                    new OrderDetailItem
                    {
                        OrderDetailId = entity.OrderDetailId,
                        ItemId = entity.ItemId,
                        SubCatId = entity.Item.SubCatId,
                        ItemName = entity.Item.ItemName,
                        AisleNumber = entity.Item.AisleNumber,
                        MaxAllowed = entity.Item.MaxAllowed,
                        PointCost = entity.Item.PointCost,
                        Quantity = entity.Quantity,
                        QuantityBefore = entity.Quantity,
                        Pulled = entity.Filled,
                        PulledBefore = entity.Filled
                    };
            }
        }

        public bool CreateOrderDetail(OrderDetailCreate model)
        {
            var entity =
                new OrderDetail()
                {
                    OrderId = model.OrderId,
                    ItemId = model.ItemId,
                    Quantity = model.Quantity,
                    Filled = model.Filled
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.OrderDetails.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool UpdateOrderDetail(OrderDetailUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .OrderDetails
                        .Single(e => e.OrderDetailId == model.OrderDetailId);
                entity.ItemId = model.ItemId;
                entity.Quantity = model.Quantity;
                entity.Filled = model.Filled;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteOrderDetail(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .OrderDetails
                        .Single(e => e.OrderDetailId == id);

                ctx.OrderDetails.Remove(entity);

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
    }
}
