using System.Linq.Expressions;
using BusinessObject;

namespace Repository;

public interface ICartRepository
{
    Task SaveAsync(Cart? cart);
    Task UpdateAsync(Cart cart);
    Task<IEnumerable<Cart?>> FindAsync(Expression<Func<Cart, bool>> predicate);
}