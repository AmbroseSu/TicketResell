using DataAccess.DTO;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService
    {
        Task<ResponseDTO> getCurrentCategories(int page, int limit);
        Task<ResponseDTO> getAllCategories(int page, int limit);
        Task<ResponseDTO> GetCategory(int id);
        Task<ResponseDTO> CreateCategory(string categoryName);
    }
}
