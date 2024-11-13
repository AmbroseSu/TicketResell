using BusinessObject;
using DataAccess.DAO;

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
}