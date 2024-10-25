using BusinessObject;

namespace Service.Impl;

public class OrderService : IOrderService
{
    public Task SaveAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetAllOrdersByUserId(long userId)
    {
        throw new NotImplementedException();
    }
}