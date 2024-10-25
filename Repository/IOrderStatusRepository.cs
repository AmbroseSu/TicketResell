using BusinessObject;

namespace Repository;

public interface IOrderStatusRepository
{
    Task SaveAsync(OrderStatus orderStatus);
    Task UpdateAsync(OrderStatus orderStatus);
    Task DeleteAsync(long orderStatusId);
    Task<OrderStatus?> FindByIdAsync(long id);
    //Task<List<OrderStatus>> FindAllOrderStatusAsync(int ticketId);
}