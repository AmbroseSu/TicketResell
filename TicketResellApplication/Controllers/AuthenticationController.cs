using System.Net;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Service;
using Service.Response;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IEmailService _emailService;

        public AuthenticationController(
            IUserService userService,
            IUserRepository userRepository,
            ILogger<AuthenticationController> logger, IAuthenticationService authenticationService, IEmailService emailService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _authenticationService = authenticationService;
            _emailService = emailService;
        }
        [HttpPost("check-email")]
        public async Task<ResponseDTO> CheckEmailAsync([FromQuery] string email)
        {
            var result = await _authenticationService.CheckEmailAsync(email);

            if (result.StatusCode.Equals(HttpStatusCode.BadRequest) )
            {
                return ResponseUtil.Error("Email is already in use", "Sign up failed", HttpStatusCode.BadRequest);
            }
            else
            {
                return result;
            }
        }

        [HttpPost("validate-email")]
        public async Task<ResponseDTO> ValidateEmailAsync([FromQuery] string token, [FromQuery] int id)
        {
            return await _authenticationService.VerifyEmailAsync(token, id);
        
        }
    
        [HttpPost("resend-otp-email")]
        public async Task<ResponseDTO> ResetCheckEmailAsync([FromQuery] string email, [FromQuery] int id)
        {
            var result = await _authenticationService.ResetVerifyEmailAsync(email, id);

            if (result.StatusCode.Equals(HttpStatusCode.BadRequest) )
            {
                return ResponseUtil.Error("Email is already in use", "Sign up failed", HttpStatusCode.BadRequest);
            }
            else
            {
                return result;
            }
        }
        
        [HttpPost("save-info")]
        public async Task<ResponseDTO> SaveInfoAsync([FromBody] SignUp signUp)
        {
            var result = await _authenticationService.SaveInfo(signUp);

            if (result.StatusCode.Equals(HttpStatusCode.BadRequest) )
            {
                return ResponseUtil.Error("Email is already in use", "Sign up failed", HttpStatusCode.BadRequest);
            }
            else
            {
                return result;
            }
        }
        
        [HttpPost("sign-in")]
        public async Task<ResponseDTO> SiginAsync([FromBody] SignInRequest signInRequest )
        {
            var result = await _authenticationService.SignIn(signInRequest);
            return result;
            
        }
        
        
        
    }
}
