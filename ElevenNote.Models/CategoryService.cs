using ElevenNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class CategoryService
    {
        public bool CreateCategory(CategoryCreate model)
        {
            var entity =
                new Category()
                {
                    //removed ID
                    Name = model.Name,
                    CreatedUtc = DateTimeOffset.Now
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CategoryListItem> GetCategory()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Categories
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new CategoryListItem
                                {
                                    CategoryId = e.CategoryId,
                                    CreatedUtc = e.CreatedUtc
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
                        .Single(e => e.OwnerId == _userId);
                return
                    new CategoryDetail
                    {
                        CategoryId = entity.CategoryId,
                        Name = entity.Name,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.CategoryId == model.CategoryId && e.OwnerId == _userId);

                entity.Name = model.Name;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int CategoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.CategoryId == CategoryId && e.OwnerId == _userId);

                ctx.Categories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }
    }
}

