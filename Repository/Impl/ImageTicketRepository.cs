using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class ImageTicketRepository : IImageTicketRepository
    {
        public async Task DeleteAsync(int id)
        {
            ImageTicket? img = (await BaseDAO<ImageTicket>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (img != null)
            {
                ImageTicket result = img;
                result.IsDeleted = true;
                await BaseDAO<ImageTicket>.Instance.UpdateAsync(result);
            }
        }

        public async Task<IEnumerable<ImageTicket?>> Find(Expression<Func<ImageTicket, bool>> predicate) => await BaseDAO<ImageTicket>.Instance.Find(predicate);

        public async Task<IEnumerable<ImageTicket>> GetAllAsync() => await BaseDAO<ImageTicket>.Instance.GetAllAsync();

        public async Task SaveAsync(ImageTicket category) => await BaseDAO<ImageTicket>.Instance.SaveAsync(category);

        public async Task UpdateAsync(ImageTicket category) => await BaseDAO<ImageTicket>.Instance.UpdateAsync(category);
    }
}
