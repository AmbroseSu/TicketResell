using System.ComponentModel.DataAnnotations;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
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
        
        [HttpGet("get-order-by-userid")]
        public async Task<ResponseDTO> GetTicketRequest([FromQuery] int userId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _orderService.GetAllOrdersByUserId(userId, page, limit);
        }

        [HttpPost("checkout-package-fee")]
        public async Task<CreatePaymentResult> CheckoutPackageFee([FromBody] PackageFeeRequset packageFeeRequset)
        {
            Transaction transaction = await _orderService.CreateTransaction(packageFeeRequset.platformFeeId, packageFeeRequset.userId,
                packageFeeRequset.quantity);
            ResponseDTO responseDto = new ResponseDTO();
            var test  = await _osService.CheckOut(_httpContextAccessor.HttpContext!.Request, transaction);
            return test;
        }
    }
}
