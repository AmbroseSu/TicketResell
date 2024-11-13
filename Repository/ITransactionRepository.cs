using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> GetAllAsync();
        public Task<IEnumerable<Transaction?>> Find(Expression<Func<Transaction, bool>> predicate);
        Task SaveAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
