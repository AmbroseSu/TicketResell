using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class TicketDAO
    {
        private readonly TicketResellDbContext _context = new TicketResellDbContext();
        private static TicketDAO instance;
        private static object instanceLock = new object();

        public TicketDAO(TicketResellDbContext context)
        {
            _context = context;
        }

        public static TicketDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TicketDAO();
                    }
                }
                return instance;
            }
        }

        public TicketDAO()
        {
            
        }

        public async Task DeleteAsync(int ticketId)
        {
            var savedTicket = await _context.Tickets.FindAsync(ticketId);
            if (savedTicket != null)
            {
                savedTicket.IsDeleted = true;
                _context.Entry<Ticket>(savedTicket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ticket?>> Find(Expression<Func<Ticket,bool>> predicate)
        {
            var result = await _context.Set<Ticket>().Where(predicate).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task SaveAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            Ticket? savedTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (savedTicket != null)
            {
                _context.Entry<Ticket>(savedTicket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

        }
    }
}
