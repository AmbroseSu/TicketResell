using BusinessObject;
using DataAccess;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class PostRepository : IPostRepository
    {
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket?>> Find(Expression<Func<Ticket, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Ticket savedTicket)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Ticket newTicket)
        {
            throw new NotImplementedException();
        }
    }
}
