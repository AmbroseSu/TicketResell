using System.ComponentModel.DataAnnotations;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Net.payOS.Types;
using Service;
using Transaction = BusinessObject.Transaction;

namespace TicketResellApplication.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly IPayOsService _osService;

        public OrderController(IOrderService orderService, IPayOsService osService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _osService = osService;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("get-order-by-userid")]
        public async Task<ResponseDTO> GetOrderByUserId([FromQuery] int userId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _orderService.GetAllOrdersByUserId(userId, page, limit);
        }
        
        [Authorize(Roles = "STAFF,ADMIN")]
        [HttpGet("get-order-by-start-day-end-date")]
        public async Task<ResponseDTO> GetOrderByStartEndDay([FromQuery] string? startDay, [FromQuery] string? endDay, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _orderService.GetAllOrdersByStartDayAndEndDay(startDay, endDay, page, limit);
        }
        [Authorize(Roles = "STAFF")]
        [HttpGet("get-order-by-order-id")]
        public async Task<ResponseDTO> GetOrderByOrderId([FromQuery] long orderId)
        {
            return await _orderService.FindOrderById(orderId);
        }

        [HttpPut("confirm-order-by-user")]
        public async Task<ResponseDTO> ConfirmOrder([FromQuery, Required] int orderId)
        {
            return await _orderService.ConfirmOrder(orderId);
        }
        
    }
}
