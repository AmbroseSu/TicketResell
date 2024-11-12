using System.Linq.Expressions;
using BusinessObject;

namespace Repository;

public interface IPlatformFeeRepository
{
    Task SaveAsync(PlatformFee platformFee);
    Task UpdateAsync(PlatformFee platformFee);
    Task DeleteAsync(long platformFeeId);
    Task<IEnumerable<PlatformFee?>> Find(Expression<Func<PlatformFee, bool>> predicate);

}