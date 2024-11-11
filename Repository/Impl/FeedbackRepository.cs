using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public Task<FeedbackResponse> AddFeedback(FeedbackDTO feedback)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFeedback(int id)
        {
            Feedback? result = (await BaseDAO<Feedback>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (result != null)
            {
                Feedback updatedResult = result;
                updatedResult.IsDeleted = true;
                await BaseDAO<Feedback>.Instance.UpdateAsync(updatedResult);
            }
        }

        public Task<IEnumerable<FeedbackResponse>> GetAllFeedbacks()
        {
            throw new NotImplementedException();
        }

        public Task<FeedbackResponse> GetFeedbackById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackResponse>> GetFeedBacksByPostId()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedbackResponse>> GetFeedBacksByTicketId()
        {
            throw new NotImplementedException();
        }
    }
}
