using System.ComponentModel.DataAnnotations;
using System.Net;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Service;
using Service.Response;
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
        private readonly ITransactionService _transactionService;

        public TransactionController(IHttpContextAccessor httpContextAccessor, IOrderService orderService, IPayOsService osService, ITransactionService transactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _osService = osService;
            _transactionService = transactionService;
        }

        [HttpPost("checkout-package-fee")]
        public async Task<ResponseDTO> CheckoutPackageFee([FromBody] PackageFeeRequset packageFeeRequset)
        {
            try
            {
                Transaction transaction = await _orderService.CreateTransaction(packageFeeRequset.platformFeeId,
                    packageFeeRequset.userId,
                    packageFeeRequset.quantity);
                
                var test = await _osService.CheckOut(_httpContextAccessor.HttpContext!.Request, transaction);
                // _osService.CheckPay(test.orderCode);
                return ResponseUtil.GetObject(test,"data",HttpStatusCode.Created,1);
            }
            catch (Exception e)
            {
                return ResponseUtil.Error(e.Message, "error", HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("get-all-transaction")]
        public async Task<ResponseDTO> GetAllTransaction([FromQuery, Required] int userId)
        {
            return await _transactionService.GetAllByuserId(userId);
        }

        [HttpGet("get-status")]
        public async Task<ResponseDTO> GetStatus([FromQuery, Required] long orderCode)
        {
            try
            {
                return await _osService.CheckPay(orderCode);
            }
            catch (Exception e)
            {
                return ResponseUtil.Error(e.Message, "error", HttpStatusCode.BadRequest);
            }

        }
        
        // [HttpGet]
    }
}
