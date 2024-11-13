using BusinessObject;
using DataAccess.DAO;
using System.Linq.Expressions;

namespace Repository.Impl;

public class OrderRepository : IOrderRepository
{
    public Task SaveAsync(Order order) => OrderDAO.Instance.SaveAsync(order);

    public Task UpdateAsync(Order order) => OrderDAO.Instance.UpdateAsync(order);

    public Task DeleteAsync(long orderId) => OrderDAO.Instance.DeleteAsync(orderId);

    public Task<Order?> FindByIdAsync(long id) => OrderDAO.Instance.FindByIdAsync(id);

    public Task<List<Order>> GetAllOrdersByUserId(long userId) => OrderDAO.Instance.GetAllOrdersByUserId(userId);
    public Task<IEnumerable<Order?>> Find(Expression<Func<Order, bool>> predicate) => OrderDAO.Instance.Find(predicate);
    public Task<List<Order>> GetAllOrdersByStartDayAndEndDay(DateTime startDay, DateTime endDay) => OrderDAO.Instance.GetAllOrdersByStartDayAndEndDay(startDay, endDay);
}