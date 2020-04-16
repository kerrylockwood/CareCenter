using CareData;
using CareModels.Catagories;
using CareModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class CategoryService
    {
        private readonly string _userId;

        public CategoryService(string userId)
        {
            _userId = userId;
        }

        public IEnumerable<CategoryList> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Categories
                        .Select(
                            e =>
                                new CategoryList
                                {
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.CategoryName,
                                }
                        );

                return query.ToArray();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.CategoryId == id);
                return
                    new CategoryDetail
                    {
                        CategoryId = entity.CategoryId,
                        CategoryName = entity.CategoryName,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity =
                new Category()
                {
                    CategoryName = model.CategoryName,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }

        public bool UpdateCategory(CategoryUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.CategoryId == model.CategoryId);
                entity.CategoryName = model.CategoryName;

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.CategoryId == id);

                ctx.Categories.Remove(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}
