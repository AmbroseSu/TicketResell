using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Service;
using Service.Impl;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowReactApp")]
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
        public async Task<ResponseDTO> RemoveTicket([FromQuery, Required] int ticketId)
        {
            return await _ticketService.DeleteTicketAsync(ticketId);
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
          [FromQuery, Required] int ticketId)
        {
            return await _ticketService.GetTicketAsync(ticketId);
        }

        [HttpGet("manager-approve")]
        public async Task<ResponseDTO> updateStatus(
            [FromQuery, Required] int ticketId,
          [FromQuery, Required] string status)
        {
            return await _ticketService.updateStatus(ticketId, status);
        }

        [HttpGet("get-by-category")]
        public async Task<ResponseDTO> getTicketByCategoryId(
         [FromQuery, Required] int categoryId,
         [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10)
        {
            return await _ticketService.getTicketByCategoryId(categoryId, page, limit);
        }

        [HttpGet("get/user")]
        public async Task<ResponseDTO> getTicket(
          [FromQuery, Required] string email,
          [FromQuery, Required] int page = 1,
          [FromQuery, Required] int limit = 10)
        {
            return await _ticketService.getTicketByEmail(email, page, limit);
        }
    }
}
