using BusinessObject;

namespace Service;

public interface IOrderService
{
    Task SaveAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(long orderId);
    Task<Order?> FindByIdAsync(long id);
    Task<List<Order>> GetAllOrdersByUserId(long userId);
}