using System.ComponentModel.DataAnnotations;
using System.Net;
using BusinessObject.enums;
using BusinessObject.Enums;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Org.BouncyCastle.Utilities;
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
                return await _transactionService.GetStatus(orderCode);
            }
            catch (Exception e)
            {
                return ResponseUtil.Error(e.Message, "error", HttpStatusCode.BadRequest);
            }

        }

        [HttpPost("update-status-transaction")]
        public async Task<ResponseDTO> UpdateStatus([FromQuery, Required] int orderCode,
            [FromQuery, Required] TransactionStatus status)
        {
            return await _transactionService.ChangeStatus(orderCode, status);
        }

        [HttpGet("get-all-admin")]
        public async Task<ResponseDTO> GetAllAdmin([FromQuery] string? startDate, [FromQuery] string? endDate,
            [FromQuery] int limt = 10, [FromQuery] int page = 1)
        {
            DateTime? startDateTime = null;
            DateTime? endDateTime = null;
            
            if (startDate != null )
            {
                int startDay = int.Parse((startDate.Split("/")[0] + startDate.Split("/")[1]));
                int startMonth = int.Parse((startDate.Split("/")[2] + startDate.Split("/")[3]));
                int startYear = int.Parse((startDate.Split("/")[4] + startDate.Split("/")[5]+startDate.Split("/")[6]+startDate.Split("/")[7]));
                startDateTime = new DateTime(startYear, startMonth, startDay);
            }
            if (endDate != null )
            {
                int endDay = int.Parse((endDate.Split("/")[0] + endDate.Split("/")[1]));
                int endMonth = int.Parse((endDate.Split("/")[2] + endDate.Split("/")[3]));
                int endYear = int.Parse((endDate.Split("/")[4] + endDate.Split("/")[5]+endDate.Split("/")[6]+endDate.Split("/")[7]));
                endDateTime = new DateTime(endYear, endMonth, endDay);
            }

            if (startDateTime == null && endDateTime == null)
            {
                return await _transactionService.GetAllTransaction(page, limt);
            }

            return null;

        }
    }
}
