using System.Linq.Expressions;

namespace DataAccess.DAO;

public interface IBaseDAO<T> where T : class
{
    Task SaveAsync(T T);
    Task UpdateAsync(T T);
    Task DeleteAsync(long id);
    Task<T?> FindByIdAsync(long id);

}