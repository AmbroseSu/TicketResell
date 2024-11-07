using BusinessObject;
using DataAccess.DTO.Response;

namespace Service;

public interface IOrderService
{
    Task SaveAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(long orderId);
    Task<Order?> FindByIdAsync(long id);
    Task<ResponseDTO> GetAllOrdersByUserId(long userId, int page, int limit);
}