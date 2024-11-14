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
    public class FavoriteController : ControllerBase
    {
        [HttpPost("add-ticket-favorite")]
        public async Task<ResponseDTO> AddTicketFavorite([FromRoute, Required] int userId,
            [FromQuery, Required] int ticketId)
        {
            return null;
        }
        
    }
}
