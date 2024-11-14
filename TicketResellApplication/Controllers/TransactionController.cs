using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Service;
using Transaction = BusinessObject.Transaction;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly IPayOsService _osService;

        public TransactionController(IHttpContextAccessor httpContextAccessor, IOrderService orderService, IPayOsService osService)
        {
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _osService = osService;
        }

        [HttpPost("checkout-package-fee")]
        public async Task<CreatePaymentResult> CheckoutPackageFee([FromBody] PackageFeeRequset packageFeeRequset)
        {
            Transaction transaction = await _orderService.CreateTransaction(packageFeeRequset.platformFeeId, packageFeeRequset.userId,
                packageFeeRequset.quantity);
            ResponseDTO responseDto = new ResponseDTO();
            var test  = await _osService.CheckOut(_httpContextAccessor.HttpContext!.Request, transaction);
            _osService.CheckPay(test.orderCode);
            return test;
        }
        
    }
}
