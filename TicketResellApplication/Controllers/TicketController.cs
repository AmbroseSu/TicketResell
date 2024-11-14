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
using BusinessObject.Enums;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "CUSTOMER")]
        [HttpPost("new")]
        public async Task<ResponseDTO> CreatePostAsync([FromBody] NewTicket post
           )
        {
            return await _ticketService.CreateTicket(post);
        }


        //[HttpPut("edit")]
        //public async Task<ResponseDTO> EditTicket([FromBody] updateTicketRequest ticket)
        //{
        //    return await _ticketService.UpdateTicketAsync(ticket);
        //}

        [Authorize(Roles = "CUSTOMER")]
        [HttpPut("remove")]
        public async Task<ResponseDTO> RemoveTicket([FromQuery, Required] int ticketId)
        {
            return await _ticketService.DeleteTicketAsync(ticketId);
        }
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("get-list")]
        public async Task<ResponseDTO> getTickets(
         [FromQuery] TicketStatus? status,
         [FromQuery] string searchTerm = "",
         [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10

         )
        {
            return await _ticketService.GetAllTicket(page, limit, status, searchTerm);
        }
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("get")]
        public async Task<ResponseDTO> getTicket(
          [FromQuery, Required] int ticketId)
        {
            return await _ticketService.GetTicketAsync(ticketId);
        }
        [Authorize(Roles = "STAFF")]
        [HttpPut("manager-action")]
        public async Task<ResponseDTO> updateStatus(
            [FromQuery, Required] int ticketId,
          [FromQuery, Required] TicketStatus status)
        {
            return await _ticketService.UpdateStatus(ticketId, status);
        }
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("get-by-category")]
        public async Task<ResponseDTO> getTicketByCategoryId(
         [FromQuery, Required] int categoryId,
         [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10)
        {
            return await _ticketService.GetTicketByCategoryId(categoryId, page, limit);
        }
        [Authorize(Roles = "CUSTOMER,STAFF")]
        [HttpGet("get-user")]
        public async Task<ResponseDTO> getTicket(
          [FromQuery, Required] int id,
          [FromQuery, Required] int page = 1,
          [FromQuery, Required] int limit = 10)
        {
            return await _ticketService.GetTicketByUserId(id, page, limit);
        }

        [HttpPost("images")]
        public async Task<ResponseDTO> AddImage([FromBody] List<string> imgList, [FromQuery, Required] int ticketId)
        {
            return await _ticketService.UpdateTicketImg(imgList, ticketId);
        }
    }
}
