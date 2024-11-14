using DataAccess.DTO;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowReactApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public TicketCategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        //[HttpGet("current-categories")]
        //public async Task<ResponseDTO> getCurrentCategories(
        //     [FromQuery, Required] int page = 1,
        //    [FromQuery, Required] int limit = 10)
        //{
        //    return await categoryService.getCurrentCategories(page, limit);
        //}
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("categories")]
        public async Task<ResponseDTO> getAllCategory(
            [FromQuery] string searchTerm = "",
            [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10

            )
        {
            return await categoryService.getAllCategories(page, limit, searchTerm);
        }
        [Authorize(Roles = "STAFF")]
        [HttpPost("new-category")]
        public async Task<ResponseDTO> CreateCategory([FromQuery, Required] string categoryName)
        {
            return await categoryService.CreateCategory(categoryName);
        }
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("category")]
        public async Task<ResponseDTO> getCategory(
           [FromQuery, Required] int id)
        {
            return await categoryService.GetCategory(id);
        }
        [Authorize(Roles = "STAFF")]
        [HttpPost("inactive")]
        public async Task<ResponseDTO> DisableCategory(
           [FromQuery, Required] int id)
        {
            return await categoryService.InactiveCategory(id);
        }
    }
}
