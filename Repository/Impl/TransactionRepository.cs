using BusinessObject;
using DataAccess.DAO;
using System.Linq.Expressions;

namespace Repository.Impl;

public class TransactionRepository : ITransactionRepository
{
    public async Task SaveAsync(Transaction transaction)
    { 
        await BaseDAO<Transaction>.Instance.SaveAsync(transaction);
    }

    public Task UpdateAsync(PlatformFee transaction)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long transactionId)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Transaction?>> Find(Expression<Func<Transaction, bool>> predicate)
        {
            return await BaseDAO<Transaction>.Instance.Find(predicate);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await BaseDAO<Transaction>.Instance.GetAllAsync();
        }
}
