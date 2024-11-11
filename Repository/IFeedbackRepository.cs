using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFeedbackRepository
    {
        public Task<IEnumerable<Feedback>> GetAllAsync();
        public Task<IEnumerable<Feedback?>> Find(Expression<Func<Feedback, bool>> predicate);
        Task SaveAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(int id);
    }
}
