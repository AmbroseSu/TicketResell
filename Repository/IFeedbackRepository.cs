using DataAccess.DTO;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbacks();
        Task<IEnumerable<FeedbackResponse>> GetFeedBacksByTicketId();
        Task<IEnumerable<FeedbackResponse>> GetFeedBacksByPostId();
        Task<FeedbackResponse> GetFeedbackById(int id);
        Task<FeedbackResponse> AddFeedback(FeedbackDTO feedback);
        Task DeleteFeedback(int id);
    }
}
