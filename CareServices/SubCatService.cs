using CareData;
using CareModels.SubCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class SubCatService
    {
        private readonly string _userId;

        public SubCatService(string userId)
        {
            _userId = userId;
        }

        public IEnumerable<SubCatList> GetSubCats()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .SubCategories
                        .Select(
                            e =>
                                new SubCatList
                                {
                                    SubCatId = e.SubCatId,
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.Category.CategoryName,
                                    SubCatName = e.SubCatName,
                                    SubCatMaxAllowed = e.SubCatMaxAllowed
                                }
                        );

                return query.ToArray();
            }
        }

        public IEnumerable<CatSubCatList> GetCatSubCats()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .SubCategories
                        .Select(
                            e =>
                                new CatSubCatList
                                {
                                    SubCatId = e.SubCatId,
                                    CatSubCatName = e.Category.CategoryName + "  --  " + e.SubCatName
                                }
                        );

                return query.OrderBy(o => o.CatSubCatName).ToArray();
            }
        }

        public SubCatDetail GetSubCatById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .SubCategories
                        .Single(e => e.SubCatId == id);
                return
                    new SubCatDetail
                    {
                        SubCatId = entity.SubCatId,
                        CategoryId = entity.CategoryId,
                        CategoryName = entity.Category.CategoryName,
                        SubCatName = entity.SubCatName,
                        SubCatMaxAllowed = entity.SubCatMaxAllowed,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public IEnumerable<SubCatListShort> GetSubCatsByCatId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .SubCategories
                        .OrderBy(e => e.SubCatName)
                        .Where(e => e.CategoryId == id)
                        .Select(
                            e =>
                                new SubCatListShort
                                {
                                    SubCatId = e.SubCatId,
                                    SubCatName = e.SubCatName,
                                    SubCatMaxAllowed = e.SubCatMaxAllowed,
                                });

                return query.ToArray();
            }
        }

        public bool CreateSubCat(SubCatCreate model)
        {
            var entity =
                new SubCategory()
                {
                    CategoryId = model.CategoryId,
                    SubCatName = model.SubCatName,
                    SubCatMaxAllowed = model.SubCatMaxAllowed,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.SubCategories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool UpdateSubCat(SubCatUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .SubCategories
                        .Single(e => e.SubCatId == model.SubCatId);
                entity.CategoryId = model.CategoryId;
                entity.SubCatName = model.SubCatName;
                entity.SubCatMaxAllowed = model.SubCatMaxAllowed;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteSubCat(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .SubCategories
                        .Single(e => e.SubCatId == id);

                ctx.SubCategories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
