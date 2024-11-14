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

        [HttpPost("add-ticket-favorite/{userId}")]
        public async Task<ResponseDTO> AddTicketFavorite([FromRoute, Required] int userId,
            [FromQuery, Required] int ticketId)
        {
            return await _favoriteService.AddTicketFavorite(userId, ticketId);
        }



        [HttpGet("get-all-favorite/{userId}")]
        public async Task<ResponseDTO> GetAllFavorite([FromRoute, Required] int userId)
        {
            return await _favoriteService.GetAllFavoriteTicketByUserId(userId);
        }

        [HttpGet("remove-favorite")]
        public async Task<ResponseDTO> RemoveFavorite([FromQuery, Required] int cartItemId)
        {
            return await _favoriteService.RemoveFavoriteTicket(cartItemId);
        }
    }
}
