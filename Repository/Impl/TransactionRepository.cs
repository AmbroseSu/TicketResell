using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class TransactionRepository : ITransactionRepository
    {

        public async Task<IEnumerable<Transaction?>> Find(Expression<Func<Transaction, bool>> predicate)
        {
            return await BaseDAO<Transaction>.Instance.Find(predicate);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await BaseDAO<Transaction>.Instance.GetAllAsync();
        }

        public async Task SaveAsync(Transaction transaction)
        {
            await BaseDAO<Transaction>.Instance.SaveAsync(transaction);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await BaseDAO<Transaction>.Instance.UpdateAsync(transaction);
        }
    }
}
