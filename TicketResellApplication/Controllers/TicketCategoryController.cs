using DataAccess.DTO;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.Generic;
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
        public async Task<ResponseDTO> getCurrentCategory(
             [FromQuery] int page = 1,
            [FromQuery] int limit = 1)
        {
            return await categoryService.getCurrentCategories(page, limit);
        }

        [HttpGet("categories")]
        public async Task<ResponseDTO> getAllCategory(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 1)
        {
            return await categoryService.getAllCategories(page,  limit);
        }

        [HttpPost("new-category")]
        public async Task<ResponseDTO> CreateCategory([FromQuery] string categoryName)
        {
            return await categoryService.CreateCategory(categoryName);
        }
    }
}
