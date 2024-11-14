using BusinessObject;
using DataAccess.DAO;
using System.Linq.Expressions;

namespace Repository.Impl;

public class OrderStatusRepository : IOrderStatusRepository
{
    public Task SaveAsync(OrderStatus orderStatus) => OrderStatusDAO.Instance.SaveAsync(orderStatus);

    public Task UpdateAsync(OrderStatus orderStatus) => OrderStatusDAO.Instance.UpdateAsync(orderStatus);

    public Task DeleteAsync(long orderStatusId) => OrderStatusDAO.Instance.DeleteAsync(orderStatusId);

    public Task<OrderStatus?> FindByIdAsync(long id) => OrderStatusDAO.Instance.FindByIdAsync(id);

    public Task<IEnumerable<OrderStatus?>> Find(Expression<Func<OrderStatus, bool>> predicate)
    {
        return BaseDAO<OrderStatus>.Instance.Find(predicate);
    }

    //public Task<List<OrderStatus>> FindAllTicketRequestsByTicketIdAsync(int ticketId)
}