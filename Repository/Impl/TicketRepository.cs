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
            Ticket? ticket = (await BaseDAO<Ticket>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (ticket != null)
            {
                Ticket updateTicket = ticket;
                updateTicket.IsDeleted = true;
                await BaseDAO<Ticket>.Instance.UpdateAsync(updateTicket);
            }
        }

        public async Task<IEnumerable<Ticket?>> Find(Expression<Func<Ticket, bool>> predicate) => await BaseDAO<Ticket>.Instance.Find(predicate);

        public async Task<IEnumerable<Ticket>> GetAllAsync() => await BaseDAO<Ticket>.Instance.GetAllAsync();

        public async Task SaveAsync(Ticket savedTicket) => await BaseDAO<Ticket>.Instance.SaveAsync(savedTicket);

        public async Task UpdateAsync(Ticket newTicket) => await BaseDAO<Ticket>.Instance.UpdateAsync(newTicket);
    }
}

