using System.ComponentModel.DataAnnotations;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketRequestController : ControllerBase
    {
        private readonly ITicketRequestService _ticketRequestService;

        public TicketRequestController(ITicketRequestService ticketRequestService)
        {
            _ticketRequestService = ticketRequestService;
        }

        [HttpPost("create-ticket-request")]
        public async Task<ResponseDTO> CreateTicketRequest([FromBody] RequestTicket requestTicket)
        {
            return await _ticketRequestService.CreateTicketRequestAsync(requestTicket);
        }
        
        [HttpGet("get-ticket-request")]
        public async Task<ResponseDTO> GetTicketRequest([FromQuery] int ticketId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _ticketRequestService.FindAllTicketRequestsByTicketIdAsync(ticketId, page, limit);
        }
        
        [HttpPost("confirm-ticket-request")]
        public async Task<ResponseDTO> ConfirmTicketRequest([FromQuery] int ticketRequestId)
        {
            return await _ticketRequestService.AcceptTicketRequestAsync(ticketRequestId);
        }
        
    }
}
