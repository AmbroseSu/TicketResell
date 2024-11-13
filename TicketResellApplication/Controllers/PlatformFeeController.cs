using System.ComponentModel.DataAnnotations;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Net.payOS.Types;
using Service;
using Transaction = BusinessObject.Transaction;

namespace TicketResellApplication.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformFeeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly IPayOsService _osService;
        private readonly IPlatformFeeService _platformFeeService;
        

        public PlatformFeeController(IOrderService orderService, IPayOsService osService, IHttpContextAccessor httpContextAccessor, IPlatformFeeService platformFeeService)
        {
            _orderService = orderService;
            _osService = osService;
            _httpContextAccessor = httpContextAccessor;
            _platformFeeService = platformFeeService;
        }
        
        [HttpGet("get-platform-fee/all")]
        public async Task<ResponseDTO> GetTicketRequest([FromQuery] string? query,
            [FromQuery] [Required] int page = 1, [FromQuery] [Required] int limit = 10)
        {
            if (query == null)
            {
                return await _platformFeeService.GetAll(page, limit);
            }

            return await _platformFeeService.GetByName(page, limit, query);
        }

    }
}
