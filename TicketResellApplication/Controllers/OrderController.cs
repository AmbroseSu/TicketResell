using System.ComponentModel.DataAnnotations;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpGet("get-order-by-userid")]
        public async Task<ResponseDTO> GetTicketRequest([FromQuery] int userId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _orderService.GetAllOrdersByUserId(userId, page, limit);
        }
    }
}
