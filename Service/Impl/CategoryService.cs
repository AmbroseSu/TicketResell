using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> getCurrentCategories(int page, int limit)
        {
            IEnumerable<Category?> result = await _categoryRepository.Find(c => c.IsDeleted == false);
            IEnumerable<Category?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "current categories retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());

        }
        
        public async Task<ResponseDTO> getAllCategories(int page, int limit)
        {
            IEnumerable<Category?> result = await _categoryRepository.GetAllAsync();
            IEnumerable<Category?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All categories retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());

        }

        public async Task<ResponseDTO> CreateCategory(string categoryName) {

            Category category = new Category()
            {
                Name = categoryName,
                IsDeleted = false
            };

            await _categoryRepository.SaveAsync(category);
            return ResponseUtil.GetObject("New category accepted","Category created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> GetCategory(int id)
        {
            IEnumerable<Category?> result = await _categoryRepository.Find(c => c.Id == id && c.IsDeleted == false);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            return ResponseUtil .GetObject(result, "Category retrieved successfully", HttpStatusCode.OK, null);
        }
    }
}
