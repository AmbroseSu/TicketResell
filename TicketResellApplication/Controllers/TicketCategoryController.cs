using DataAccess.DTO;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public TicketCategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("current-categories")]
        public async Task<ResponseDTO> getCurrentCategories(
             [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await categoryService.getCurrentCategories(page, limit);
        }

        [HttpGet("categories")]
        public async Task<ResponseDTO> getAllCategory(
            [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await categoryService.getAllCategories(page,  limit);
        }

        [HttpPost("new-category")]
        public async Task<ResponseDTO> CreateCategory([FromQuery, Required] string categoryName)
        {
            return await categoryService.CreateCategory(categoryName);
        }

        [HttpGet("category")]
        public async Task<ResponseDTO> getCategory(
           [FromQuery, Required] int id)
        {
            return await categoryService.GetCategory(id);
        }
    }
}
