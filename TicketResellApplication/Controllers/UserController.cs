using System.ComponentModel.DataAnnotations;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowReactApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFeedbackService _feedbackService;
        
        public UserController(IUserService userService, IFeedbackService feedbackService)
        {
            _userService = userService;
            _feedbackService = feedbackService;
        }

        [HttpGet("get-user-by-email")]
        public async Task<ResponseDTO> GetUserByEmail([FromQuery] string email)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.GetUserByEmailAsync(email);
        }
        
        [HttpGet("get-all-user-by-role")]
        public async Task<ResponseDTO> GetAllUserByRole([FromQuery] Role role, [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindAllByRoleAsync(role, page, limit);
        }
        
        [HttpGet("get-all-user")]
        public async Task<ResponseDTO> GetAllUser([FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindAllUsersAsync(page, limit);
        }
        
        [HttpGet("get-all-user-by-month-and-year")]
        public async Task<ResponseDTO> GetAllUserByMonthAndYear([FromQuery] int month, [FromQuery] int year, [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindAllCustomersByDateAndYearAsync(month, year, page, limit);
        }
        
                
        [HttpGet("get-all-number-of-user-by-month-and-year")]
        public async Task<ResponseDTO> GetAllNumberOfUserByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindAllNumberOfCustomersByDateAndYearAsync(month, year);
        }
        
        [HttpGet("get-user-by-id")]
        public async Task<ResponseDTO> GetUserById([FromQuery] long id)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindUserByIdAsync(id);
        }
        
        [HttpPost("edit-profile-user")]
        public async Task<ResponseDTO> EditProfileUser([FromBody] UpsertUserDTO upsertUserDto)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.EditProfileAsync(upsertUserDto);
        }
        
        [HttpGet("search")]
        public async Task<ResponseDTO> SearchUser([FromQuery] string search, [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.SearchUsersByEmailAndFullNameAsync(search, page, limit);
        }
        
        [HttpGet("change-active")]
        public async Task<ResponseDTO> ChangeActiveUser([FromQuery] int id)
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.ChangeActiveUserAsync(id);
        }
        
        [HttpGet("get-number-gender")]
        public async Task<ResponseDTO> GetNumberGender()
        {
            await _feedbackService.UpdateUserReputation();
            return await _userService.FindByGenderAsync();
        }
        
    }
}
