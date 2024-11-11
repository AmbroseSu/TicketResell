using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public async Task DeleteAsync(int id)
        {
            Feedback? result = (await BaseDAO<Feedback>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (result != null)
            {
                Feedback updatedResult = result;
                updatedResult.IsDeleted = true;
                await BaseDAO<Feedback>.Instance.UpdateAsync(updatedResult);
            }
        }

        public async Task<IEnumerable<Feedback?>> Find(Expression<Func<Feedback, bool>> predicate)
        {
           return await BaseDAO<Feedback>.Instance.Find(predicate);
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await BaseDAO<Feedback>.Instance.GetAllAsync();
        }

        public async Task SaveAsync(Feedback feedback)
        {
            await BaseDAO<Feedback>.Instance.SaveAsync(feedback);
        }

        public async Task UpdateAsync(Feedback feedback)
        {
            await BaseDAO<Feedback>.Instance.UpdateAsync(feedback);
        }
    }
}
