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
    }
}
