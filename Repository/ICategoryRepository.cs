using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICategoryRepository 
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category?>> Find(Expression<Func<Category, bool>> predicate);
        Task SaveAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
