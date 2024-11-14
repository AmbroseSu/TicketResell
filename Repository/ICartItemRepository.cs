using System.Linq.Expressions;
using BusinessObject;

namespace Repository;

public interface ICartItemRepository
{
    Task SaveAsync(CartItem CartItem);
    Task UpdateAsync(CartItem CartItem);
    Task<IEnumerable<CartItem?>> FindAsync(Expression<Func<CartItem, bool>> predicate);
}