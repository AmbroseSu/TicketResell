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
        public async Task DeleteAsync(int id)
        {
            Post? result = (await BaseDAO<Post>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (result != null)
            {
                Post updateResult = result;
                updateResult.IsDeleted = true;
                await BaseDAO<Post>.Instance.UpdateAsync(updateResult);
            }
        }

        public async Task<IEnumerable<Post?>> Find(Expression<Func<Post, bool>> predicate) => await BaseDAO<Post>.Instance.Find(predicate);

        public async Task<IEnumerable<Post>> GetAllAsync() => await BaseDAO<Post>.Instance.GetAllAsync();

        public async Task SaveAsync(Post savedTicket) => await BaseDAO<Post>.Instance.SaveAsync(savedTicket);

        public async Task UpdateAsync(Post newTicket) => await BaseDAO<Post>.Instance.UpdateAsync(newTicket);
    }
}
