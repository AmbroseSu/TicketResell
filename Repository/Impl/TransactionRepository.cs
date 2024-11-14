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

    public async Task UpdateAsync(Transaction transaction)
    {
        await BaseDAO<Transaction>.Instance.UpdateAsync(transaction);
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

        public async Task<IEnumerable<Transaction?>> FindAll<TKey>(Expression<Func<Transaction, TKey>> predicate) =>
            await BaseDAO<Transaction>.Instance.GetAllAsync(predicate);
}