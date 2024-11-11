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
        public async Task<bool> AddCategory(String name)
        {
            Category? result = (await BaseDAO<Category>.Instance.Find(c => c.Name.Trim().Equals(name.Trim()))).SingleOrDefault();

            if (result == null)
            {
                Category category = new Category();
                category.Name = name;
                await BaseDAO<Category>.Instance.SaveAsync(category);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Category? category = (await BaseDAO<Category>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (category != null)
            {
                Category updateCate = category;
                updateCate.IsDeleted = true;
                await BaseDAO<Category>.Instance.UpdateAsync(updateCate);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Category?>> Find(Expression<Func<Category, bool>> predicate) => await BaseDAO<Category>.Instance.Find(predicate);

        public async Task<IEnumerable<Category>> GetAllAsync() => await BaseDAO<Category>.Instance.GetAllAsync();

        public async Task SaveAsync(Category category)
        {
            await BaseDAO<Category>.Instance.SaveAsync(category);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                await BaseDAO<Category>.Instance.UpdateAsync(category);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
