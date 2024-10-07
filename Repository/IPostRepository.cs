using BusinessObject;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPostRepository 
    {
        public Task<IEnumerable<Ticket>> GetAllAsync();
        public Task<IEnumerable<Ticket?>> Find(Expression<Func<Ticket, bool>> predicate);
        Task SaveAsync(Ticket savedTicket);
        Task UpdateAsync(Ticket newTicket);
        Task DeleteAsync(int id);

    }
}
