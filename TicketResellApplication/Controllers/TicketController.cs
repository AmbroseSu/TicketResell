using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Service;
using Service.Impl;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ResponseDTO> CreateTicket([FromBody] NewTicketRequest ticket
            )
        {
            return await _ticketService.CreateTicketAsync(ticket);
        }

        [HttpPut("edit")]
        public async Task<ResponseDTO> EditTicket([FromBody] updateTicketRequest ticket)
        {
            return await _ticketService.UpdateTicketAsync(ticket);
        }


        [HttpPut("remove")]
        public async Task<ResponseDTO> RemoveTicket([FromQuery, Required] int id)
        {
            return await _ticketService.DeleteTicketAsync(id);
        }

        [HttpGet("get-list")]
        public async Task<ResponseDTO> getTickets(
         [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10)
        {
            return await _ticketService.GetTicketsAsync(page, limit);
        }

        [HttpGet("get")]
        public async Task<ResponseDTO> getTicket(
          [FromQuery, Required] int id)
        {
            return await _ticketService.GetTicketAsync(id);
        }

        [HttpGet("status-manage")]
        public async Task<ResponseDTO> updateStatus(
            [FromQuery, Required] int id,
          [FromQuery, Required] string status)
        {
            return await _ticketService.updateStatus(id, status);
        }
    }
}
