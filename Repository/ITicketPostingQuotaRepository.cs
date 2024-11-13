using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITicketPostingQuotaRepository
    {
        public Task<IEnumerable<TicketPostingQuota>> GetAllAsync();
        public Task<IEnumerable<TicketPostingQuota?>> Find(Expression<Func<TicketPostingQuota, bool>> predicate);
        Task SaveAsync(TicketPostingQuota ticketPostingQuota);
        Task UpdateAsync(TicketPostingQuota ticketPostingQuota);
    }
}
