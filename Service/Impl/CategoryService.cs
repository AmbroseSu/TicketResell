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
using System.Text.RegularExpressions;
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
            return ResponseUtil.GetCollection(data, "current categories retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());

        }

        public async Task<ResponseDTO> getAllCategories(int page, int limit, String? searchTerm)
        {
            IEnumerable<Category?> result = await _categoryRepository.Find(c => c.Name.ToLower().Contains(searchTerm.ToLower().Trim()));
            IEnumerable<Category?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All categories retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());

        }

        public async Task<ResponseDTO> CreateCategory(string categoryName)
        {
            if (categoryName == null || !Regex.IsMatch(categoryName.Trim(), @"^[a-zA-Z 1-9]+$"))
            {
                return ResponseUtil.Error("Request fails", "Category name must contain only letters or number", HttpStatusCode.BadRequest);
            }

            Category? category = (await _categoryRepository.Find(c => c.Name.ToLower().Equals(categoryName.Trim().ToLower()))).SingleOrDefault();

            if (category != null)
            {
                return ResponseUtil.Error("Request fails", "Category already exists", HttpStatusCode.BadRequest);
            }
            category = new Category();
            category.Name = categoryName;
            category.IsDeleted = false;

            await _categoryRepository.SaveAsync(category);
            return ResponseUtil.GetObject("New category accepted", "Category created successfully", HttpStatusCode.OK, 0);
        }

        public async Task<ResponseDTO> GetCategory(int id)
        {
            IEnumerable<Category?> result = await _categoryRepository.Find(c => c.Id == id && c.IsDeleted == false);

            if (result.Count() == 0)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            return ResponseUtil.GetObject(result, "Category retrieved successfully", HttpStatusCode.OK, 0);
        }

        public async Task<ResponseDTO> SearchCategory(string searchTerm)
        {
            IEnumerable<Category?> categories = await _categoryRepository.Find(c => c.Name.Contains(searchTerm.ToLower().Trim()));
            List<CategoryDTO> result = new List<CategoryDTO>();
            if (categories.Count() != 0)
            {
                foreach (Category item in categories)
                {
                    result.Add(_mapper.Map<CategoryDTO>(item));
                }
            }

            return ResponseUtil.GetCollection(result, "Categories retrieved successfully", HttpStatusCode.OK, categories.Count(), 1, categories.Count(), categories.Count());
        }

        public async Task<ResponseDTO> InactiveCategory(int id)
        {
            Category? category = (await _categoryRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            if (category.IsDeleted == true)
            {
                category.IsDeleted = false;
            }
            else
            {
                category.IsDeleted = true;
            }
            _categoryRepository.UpdateAsync(category);
            return ResponseUtil.GetObject(category, "Category updated successfully", HttpStatusCode.OK, 1);
        }
    }
}
