using CareData;
using CareModels.Orders;
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
                                    //SlotDateTime = ConvertSlotToDateTime(e.SlotId, e.CreatedAt.DateTime),
                                    CustFirstName = e.Customer.FirstName,
                                    CustLastName = e.Customer.LastName,
                                    Deliver = e.Deliver,
                                    PullStarted = (e.PullStartedAt == null) ? false : true,
                                    PullCompleted = (e.OrderCompletedAt == null) ? false : true
                                }
                        );

                var slotArray = query.ToArray();
                foreach (var item in slotArray)
                {
                    item.SlotDateTime = ConvertSlotToDateTime(item.SlotId, item.CreateDateTime.DateTime);
                }
                return slotArray;
            }
        }

        //public ItemDetail GetItemById(int id)
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        var entity =
        //            ctx
        //                .Items
        //                .Single(e => e.ItemId == id);
        //        return
        //            new ItemDetail
        //            {
        //                ItemId = entity.ItemId,
        //                SubCatId = entity.SubCatId,
        //                //CategoryId = entity.SubCategory.CategoryId,
        //                CategoryName = entity.SubCategory.Category.CategoryName,
        //                SubCatName = entity.SubCategory.SubCatName,
        //                ItemName = entity.ItemName,
        //                IsleNumber = entity.IsleNumber,
        //                MaxAllowed = entity.MaxAllowed,
        //                PointCost = entity.PointCost,
        //                CreateAt = entity.CreateAt,
        //                CreateName = entity.User.UserName
        //            };
        //    }
        //}

        //public bool CreateItem(ItemCreate model)
        //{
        //    var entity =
        //        new Item()
        //        {
        //            //ItemId = model.ItemId,
        //            SubCatId = model.SubCatId,
        //            ItemName = model.ItemName,
        //            IsleNumber = model.IsleNumber,
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
        //        entity.IsleNumber = model.IsleNumber;
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

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime ConvertSlotToDateTime(int slotId, DateTime createDateTime)
        {
            //*******get slot info here
            TimeSpan Time = new TimeSpan(19, 00, 00);
            int DayOfWeek = 7;
            //*******get slot info here

            DateTime weekStartDate = GetWeekStartDate(createDateTime);

            return weekStartDate.AddDays(DayOfWeek).Add(Time);
        }

        // Get Slot Date/Time from Slot DayOfWeek
        public DateTime GetWeekStartDate(DateTime createDateTime)
        {
            // Assumption: Customers can start creating orders on the
            //    day after the last Pickup Slot day and Pickups start
            //    as early as Sunday
            // Get Sunday Date based on date passed in (should be either
            //    Create Date or current date if creating now)
            //*******get *last* slot here
            TimeSpan lastSlotTime = new TimeSpan(19, 00, 00);
            int lastSlotDayOfWeek = 7;
            //*******get *last* slot here

            int createDayOfWeek = (int)createDateTime.DayOfWeek;
            DateTime weekStartDate
                = createDateTime.Date.AddDays(createDayOfWeek * -1);
            DateTime orderCutoffTime = weekStartDate.AddDays(lastSlotDayOfWeek).Add(lastSlotTime).AddMinutes(-60);

            if (createDateTime > orderCutoffTime)
            {
                // Created after last appointment slot (less buffer)
                // Need to set Start Date to following Sunday
                weekStartDate = weekStartDate.AddDays(7);
            }

            return weekStartDate;
        }
    }
}
