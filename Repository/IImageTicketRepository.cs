using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IImageTicketRepository
    {
        Task<IEnumerable<ImageTicket>> GetAllAsync();
        Task<IEnumerable<ImageTicket?>> Find(Expression<Func<ImageTicket, bool>> predicate);
        Task SaveAsync(ImageTicket category);
        Task UpdateAsync(ImageTicket category);
        Task DeleteAsync(int id);
    }
}
