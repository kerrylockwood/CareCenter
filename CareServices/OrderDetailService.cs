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

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
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

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
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

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}
