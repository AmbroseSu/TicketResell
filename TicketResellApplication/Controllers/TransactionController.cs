using System.ComponentModel.DataAnnotations;
using System.Net;
using BusinessObject.enums;
using BusinessObject.Enums;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "CUSTOMER")]
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
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("get-all-transaction")]
        public async Task<ResponseDTO> GetAllTransaction([FromQuery, Required] int userId)
        {
            return await _transactionService.GetAllByuserId(userId);
        }
        [Authorize(Roles = "CUSTOMER")]
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
        [Authorize(Roles = "CUSTOMER")]
        [HttpPost("update-status-transaction")]
        public async Task<ResponseDTO> UpdateStatus([FromQuery, Required] int orderCode,
            [FromQuery, Required] TransactionStatus status)
        {
            return await _transactionService.ChangeStatus(orderCode, status);
        }
        [Authorize(Roles = "ADMIN,STAFF")]
        [HttpGet("get-all-admin")]
        public async Task<ResponseDTO> GetAllAdmin([FromQuery] string? startDate, [FromQuery] string? endDate,
            [FromQuery] int limt = 10, [FromQuery] int page = 1)
        {
            
            if (startDate == null && endDate == null)
            {
                return await _transactionService.GetAllTransaction(page, limt);
            }
            else
            {
                return await _transactionService.GetAllTransactionWithDate(page, limt, startDate, endDate);
            }

        }
        [Authorize(Roles = "STAFF,ADMIN")]
        [HttpGet("get-by-id-admin")]
        public async Task<ResponseDTO> GetByIdAdmin([FromQuery, Required] int transId)
        {
            return await _transactionService.GettransactionById(transId);
        }
        [Authorize(Roles = "STAFF,ADMIN")]
        [HttpGet("total-revenue")]
        public async Task<ResponseDTO> GetTotalRevenue()
        {
            return await _transactionService.GetToTalRevenue();
        }
        [Authorize(Roles = "ADMIN,STAFF")]
        [HttpGet("get-top-five")]
        public async Task<ResponseDTO> GetTopFive()
        {
            return await _transactionService.GetFiveTopTransaction();
        }
    }
}
