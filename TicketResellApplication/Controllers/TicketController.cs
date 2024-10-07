using DataAccess.DTO;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Service;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("new-ticket")]
        public async Task<ResponseDTO> CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            return await _ticketService.CreateTicketAsync(ticketDTO);
        }

        [HttpPut("edit-ticket")]
        public async Task<ResponseDTO> EditTicket([FromBody] TicketDTO ticket)
        {
            return await _ticketService.UpdateTicketAsync(ticket);
        }


        [HttpDelete("remove-ticket")]
        public async Task<ResponseDTO> RemoveTicket([FromQuery] int id)
        {
            return await _ticketService.DeleteTicketAsync(id);
        }
    }
}
