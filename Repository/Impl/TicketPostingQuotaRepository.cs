using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class TicketPostingQuotaRepository : ITicketPostingQuotaRepository
    {
        
        public async Task<IEnumerable<TicketPostingQuota?>> Find(Expression<Func<TicketPostingQuota, bool>> predicate)
        {
            return await BaseDAO<TicketPostingQuota>.Instance.Find(predicate);
        }

        public async Task<IEnumerable<TicketPostingQuota>> GetAllAsync()
        {
            return await BaseDAO<TicketPostingQuota>.Instance.GetAllAsync();
        }

        public async Task SaveAsync(TicketPostingQuota ticketPostingQuota)
        {
            await BaseDAO<TicketPostingQuota>.Instance.SaveAsync(ticketPostingQuota);
        }

        public async Task UpdateAsync(TicketPostingQuota ticketPostingQuota)
        {
            await BaseDAO<TicketPostingQuota>.Instance.UpdateAsync(ticketPostingQuota);
        }
    }
}
