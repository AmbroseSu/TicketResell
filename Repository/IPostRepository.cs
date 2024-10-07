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
        public Task<IEnumerable<Post>> GetAllAsync();
        public Task<IEnumerable<Post?>> Find(Expression<Func<Post, bool>> predicate);
        Task SaveAsync(Post savedTicket);
        Task UpdateAsync(Post newTicket);
        Task DeleteAsync(int id);

    }
}
