using System.Linq.Expressions;
using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class PlatformFeeRepository : IPlatformFeeRepository
{
    
    public async Task SaveAsync(PlatformFee platformFee)
    {
        await BaseDAO<PlatformFee>.Instance.SaveAsync(platformFee);
    }

    public Task UpdateAsync(PlatformFee platformFee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long platformFeeId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PlatformFee?>> Find(Expression<Func<PlatformFee, bool>> predicate)
    {
       return await BaseDAO<PlatformFee>.Instance.Find(predicate);
       
    }
}