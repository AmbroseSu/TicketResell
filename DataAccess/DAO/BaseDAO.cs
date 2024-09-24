namespace DataAccess.DAO;

public interface BaseDAO<T>
{
    Task SaveAsync(T T);
    Task UpdateAsync(T T);
    Task DeleteAsync(long id);
    Task<T?> FindByIdAsync(long id);
}