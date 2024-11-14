using System.Linq.Expressions;
using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class TransactionRepository : ITransactionRepository
{
    public async Task SaveAsync(Transaction transaction)
    { 
        await BaseDAO<Transaction>.Instance.SaveAsync(transaction);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        await BaseDAO<Transaction>.Instance.SaveAsync(transaction);
    }

    public Task DeleteAsync(long transactionId)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<IEnumerable<Transaction?>> Find(Expression<Func<Transaction, bool>> predicate) => await BaseDAO<Transaction>.Instance.Find(predicate);
}