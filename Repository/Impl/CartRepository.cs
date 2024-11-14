using System.Linq.Expressions;
using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class CartRepository : ICartRepository
{
    public async Task SaveAsync(Cart? cart) => await BaseDAO<Cart>.Instance.SaveAsync(cart);
    public async Task UpdateAsync(Cart cart) => await BaseDAO<Cart>.Instance.UpdateAsync(cart);

    public async Task<IEnumerable<Cart?>> FindAsync(Expression<Func<Cart, bool>> predicate) =>
        await BaseDAO<Cart>.Instance.Find(predicate);
}