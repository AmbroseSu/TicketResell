using System.ComponentModel.DataAnnotations;
using System.Net;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Repository;
using Service;
using Service.Response;
using Transaction = BusinessObject.Transaction;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("add-ticket-favorite")]
        public async Task<ResponseDTO> AddTicketFavorite([FromRoute, Required] int userId,
            [FromQuery, Required] int ticketId)
        {
            return await _favoriteService.AddTicketFavorite(userId, ticketId);
        }
        
    }
}
