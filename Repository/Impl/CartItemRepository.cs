using System.Linq.Expressions;
using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class CartItemRepository : ICartItemRepository
{
    public async Task SaveAsync(CartItem CartItem) => await BaseDAO<CartItem>.Instance.SaveAsync(CartItem);
    public async Task UpdateAsync(CartItem CartItem) => await BaseDAO<CartItem>.Instance.UpdateAsync(CartItem);

    public async Task<IEnumerable<CartItem?>> FindAsync(Expression<Func<CartItem, bool>> predicate) =>
        await BaseDAO<CartItem>.Instance.Find(predicate);
}