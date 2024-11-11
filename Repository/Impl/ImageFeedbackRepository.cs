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
    public class ImageFeedbackRepository : IImageFeedbackRepository
    {
        public async Task DeleteAsync(int id)
        {
            ImageFeedback? img = (await BaseDAO<ImageFeedback>.Instance.Find(c => c.Id == id && c.IsDeleted == false)).SingleOrDefault();

            if (img != null)
            {
                ImageFeedback result = img;
                result.IsDeleted = true;
                await BaseDAO<ImageFeedback>.Instance.UpdateAsync(result);
            }
        }

        public async Task<IEnumerable<ImageFeedback?>> Find(Expression<Func<ImageFeedback, bool>> predicate)
        {
            return await BaseDAO<ImageFeedback>.Instance.Find(predicate);
        }

        public async Task<IEnumerable<ImageFeedback>> GetAllAsync()
        {
            return await BaseDAO<ImageFeedback>.Instance.GetAllAsync();
        }

        public async Task SaveAsync(ImageFeedback img)
        {
            await BaseDAO<ImageFeedback>.Instance.SaveAsync(img);
        }

        public async Task UpdateAsync(ImageFeedback img)
        {
            await BaseDAO<ImageFeedback>.Instance.UpdateAsync(img);
        }
    }
}
