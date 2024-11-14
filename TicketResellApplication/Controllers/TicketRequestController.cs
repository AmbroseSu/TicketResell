using System.ComponentModel.DataAnnotations;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowReactApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketRequestController : ControllerBase
    {
        private readonly ITicketRequestService _ticketRequestService;

        public TicketRequestController(ITicketRequestService ticketRequestService)
        {
            _ticketRequestService = ticketRequestService;
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpPost("create-ticket-request")]
        public async Task<ResponseDTO> CreateTicketRequest([FromBody] RequestTicket requestTicket)
        {
            return await _ticketRequestService.CreateTicketRequestAsync(requestTicket);
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("get-ticket-request")]
        public async Task<ResponseDTO> GetTicketRequest([FromQuery] int ticketId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _ticketRequestService.FindAllTicketRequestsByTicketIdAsync(ticketId, page, limit);
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("get-ticket-request-for-buyer")]
        public async Task<ResponseDTO> GetTicketRequestForBuyer([FromQuery] int userId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _ticketRequestService.FindAllTicketRequestsByUserIdAsync(userId, page, limit);
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpPost("confirm-ticket-request")]
        public async Task<ResponseDTO> ConfirmTicketRequest([FromQuery] int ticketRequestId)
        {
            return await _ticketRequestService.AcceptTicketRequestAsync(ticketRequestId);
        }
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("get-ticket-request-by-id")]
        public async Task<ResponseDTO> GetTicketRequestById([FromQuery] long ticketRequestId)
        {
            return await _ticketRequestService.FindByIdAsync(ticketRequestId);
        }
        
        
        
    }
}
