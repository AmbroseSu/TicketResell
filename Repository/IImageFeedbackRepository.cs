using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IImageFeedbackRepository
    {
        Task<IEnumerable<ImageFeedback>> GetAllAsync();
        Task<IEnumerable<ImageFeedback?>> Find(Expression<Func<ImageFeedback, bool>> predicate);
        Task SaveAsync(ImageFeedback category);
        Task UpdateAsync(ImageFeedback category);
        Task DeleteAsync(int id);
    }
}
