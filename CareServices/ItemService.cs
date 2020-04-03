using CareData;
using CareModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class ItemService
    {
        private readonly string _userId;

        public ItemService(string userId)
        {
            _userId = userId;
        }

        public IEnumerable<ItemList> GetItems()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Items
                        .Select(
                            e =>
                                new ItemList
                                {
                                    ItemId = e.ItemId,
                                    SubCatId = e.SubCatId,
                                    CategoryId = e.SubCategory.CategoryId,
                                    CategoryName = e.SubCategory.Category.CategoryName,
                                    SubCatName = e.SubCategory.SubCatName,
                                    ItemName = e.ItemName,
                                    IsleNumber = e.IsleNumber,
                                    MaxAllowed = e.MaxAllowed,
                                    PointCost = e.PointCost
                                }
                        );

                return query.ToArray();
            }
        }

        public ItemDetail GetItemById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Items
                        .Single(e => e.ItemId == id);
                return
                    new ItemDetail
                    {
                        ItemId = entity.ItemId,
                        SubCatId = entity.SubCatId,
                        //CategoryId = entity.SubCategory.CategoryId,
                        CategoryName = entity.SubCategory.Category.CategoryName,
                        SubCatName = entity.SubCategory.SubCatName,
                        ItemName = entity.ItemName,
                        IsleNumber = entity.IsleNumber,
                        MaxAllowed = entity.MaxAllowed,
                        PointCost = entity.PointCost,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public bool CreateItem(ItemCreate model)
        {
            var entity =
                new Item()
                {
                    //ItemId = model.ItemId,
                    SubCatId = model.SubCatId,
                    ItemName = model.ItemName,
                    IsleNumber = model.IsleNumber,
                    MaxAllowed = model.MaxAllowed,
                    PointCost = model.PointCost,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Items.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool UpdateItem(ItemUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Items
                        .Single(e => e.ItemId == model.ItemId);
                entity.SubCatId = model.SubCatId;
                entity.ItemName = model.ItemName;
                entity.IsleNumber = model.IsleNumber;
                entity.MaxAllowed = model.MaxAllowed;
                entity.PointCost = model.PointCost;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteItem(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Items
                        .Single(e => e.ItemId == id);

                ctx.Items.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
