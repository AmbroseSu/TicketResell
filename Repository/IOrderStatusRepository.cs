using BusinessObject;
using System.Linq.Expressions;

namespace Repository;

public interface IOrderStatusRepository
{
    Task SaveAsync(OrderStatus orderStatus);
    Task UpdateAsync(OrderStatus orderStatus);
    Task DeleteAsync(long orderStatusId);
    Task<OrderStatus?> FindByIdAsync(long id);
    //Task<List<OrderStatus>> FindAllOrderStatusAsync(int ticketId);
    Task<IEnumerable<OrderStatus?>> Find(Expression<Func<OrderStatus, bool>> predicate);
    Task<List<OrderStatus>> GetAllOrdersByOrderId(int orderId);
}