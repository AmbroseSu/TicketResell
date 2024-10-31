using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("get-user-by-email")]
        public async Task<ResponseDTO> GetUserByEmail([FromQuery] string email)
        {
            return await _userService.GetUserByEmailAsync(email);
        }
    }
}
