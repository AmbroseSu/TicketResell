using BusinessObject;
using System.Linq.Expressions;

namespace Repository;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(long orderId);
    Task<Order?> FindByIdAsync(long id);
    Task<List<Order>> GetAllOrdersByUserId(long userId);
    Task<IEnumerable<Order?>> Find(Expression<Func<Order, bool>> predicate);
    Task<List<Order>> GetAllOrdersByStartDayAndEndDay(DateTime startDay, DateTime endDay);
    Task<List<Order>> GetAllOrders();
    Task<IEnumerable<Order?>> FindAsync(Expression<Func<Order, bool>> predicate);

}