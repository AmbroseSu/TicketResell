using BusinessObject;
using DataAccess;
using DataAccess.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class TicketRepository : ITicketRepository
    {
        public async Task DeleteAsync(int id)
        {
            IEnumerable<Ticket?> ticket = await BaseDAO<Ticket>.Instance.Find(c => c.Id == id && c.IsDeleted==false);

            if (ticket != null)
            {
                Ticket updateTicket = ticket.First();
                updateTicket.IsDeleted = true;
                await BaseDAO<Ticket>.Instance.UpdateAsync(updateTicket);
            }
        }

        public Task<IEnumerable<Ticket?>> Find(Expression<Func<Ticket, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync() => await BaseDAO<Ticket>.Instance.GetAllAsync();

        public async Task SaveAsync(Ticket savedTicket) => await BaseDAO<Ticket>.Instance.SaveAsync(savedTicket);

        public async Task UpdateAsync(Ticket newTicket) => await BaseDAO<Ticket>.Instance.UpdateAsync(newTicket);
    }
}

