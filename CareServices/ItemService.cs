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
                                    AisleNumber = e.AisleNumber,
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
                        AisleNumber = entity.AisleNumber,
                        MaxAllowed = entity.MaxAllowed,
                        PointCost = entity.PointCost,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public IEnumerable<ItemListShort> GetItemsBySubCatId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Items
                        .OrderBy(e => e.ItemName)
                        .Where(e => e.SubCatId == id)
                        .Select(
                            e =>
                                new ItemListShort
                                {
                                    ItemId = e.ItemId,
                                    ItemName = e.ItemName,
                                    AisleNumber = e.AisleNumber,
                                    MaxAllowed = e.MaxAllowed,
                                    PointCost = e.PointCost
                                }
                                );

                return query.ToArray();
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
                    AisleNumber = model.AisleNumber,
                    MaxAllowed = model.MaxAllowed,
                    PointCost = model.PointCost,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Items.Add(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
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
                entity.AisleNumber = model.AisleNumber;
                entity.MaxAllowed = model.MaxAllowed;
                entity.PointCost = model.PointCost;

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
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

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}
