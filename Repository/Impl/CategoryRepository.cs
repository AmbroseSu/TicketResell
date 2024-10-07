using BusinessObject;
using DataAccess;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task DeleteAsync(int id)
        {
            Category? category = (await BaseDAO<Category>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (category != null)
            {
                Category updateCate = category;
                updateCate.IsDeleted = true;
                await BaseDAO<Category>.Instance.UpdateAsync(updateCate);
            }

        }

        public async Task<IEnumerable<Category?>> Find(Expression<Func<Category, bool>> predicate) => await BaseDAO<Category>.Instance.Find(predicate);

        public async Task<IEnumerable<Category>> GetAllAsync() => await BaseDAO<Category>.Instance.GetAllAsync();

        public async Task SaveAsync(Category category)
        {
            await BaseDAO<Category>.Instance.SaveAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            await BaseDAO<Category>.Instance.UpdateAsync(category);
        }
    }
}
