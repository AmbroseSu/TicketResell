using System.Linq.Expressions;
using BusinessObject;

namespace Repository;

public interface ITransactionRepository
{
    Task SaveAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(long transactionId);
    Task<Transaction?> FindByIdAsync(long id);
    Task<IEnumerable<Transaction?>> Find(Expression<Func<Transaction, bool>> predicate);

}