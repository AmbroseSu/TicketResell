using System.Net;
using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class AuthenticationService : IAuthenticationService
{
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        /*private readonly IPasswordHasher<User> _passwordHasher;
        private readonly SignInManager<User> _signInManager;*/
        private readonly IJwtService _jwtService;
        private readonly IVerificationTokenRepository _verificationTokenRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthenticationService(IUserRepository userRepository, IUserService userService, 
            /*IPasswordHasher<User> passwordHasher, SignInManager<User> signInManager, */
            IJwtService jwtService,
            IVerificationTokenRepository verificationTokenRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _userService = userService;
            /*_passwordHasher = passwordHasher;
            _signInManager = signInManager;*/
            _jwtService = jwtService;
            _verificationTokenRepository = verificationTokenRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ResponseDTO> CheckEmailAsync(string email)
        {
            try
            {
                if (await _userRepository.ExistsByEmailAsync(email) && (await _userRepository.FindUserByEmailAsync(email)).IsEnabled)
                {
                    User userCheck = await _userRepository.FindUserByEmailAsync(email);
                    int userId = userCheck.Id;
                    userCheck.VerificationTokenId = null;
                    userCheck.VerificationToken = null;
                    await _userRepository.UpdateAsync(userCheck);
                    int verificationId = (await _verificationTokenRepository.FindByUserIdAsync(userId)).Id;
                    await _verificationTokenRepository.DeleteAsync(verificationId);
                    return ResponseUtil.Error("Email is already in use", "Failed", HttpStatusCode.BadRequest);
                }
                
                var user = new User()
                {
                    Email = email,
                    IsEnabled = false,
                    IsDeleted = false
                };

                await _userRepository.SaveAsync(user);
                
                var sendEmail = await _emailService.SendEmail(email);
                if (sendEmail.StatusCode.Equals(HttpStatusCode.BadRequest) )
                {
                    return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                }
                
                var result = _mapper.Map<UpsertUserDTO>(user);

                return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> VerifyEmailAsync(string token, int id)
        {
            try
            {
                VerificationToken theToken = await _verificationTokenRepository.FindByTokenAsync(token);

                if (theToken == null)
                {
                    return ResponseUtil.Error("Token not exist", "Failed", HttpStatusCode.BadRequest);
                }
                if (theToken.User != null && theToken.User.IsEnabled == true)
                {
                    return ResponseUtil.Error("This account has already been verified, please, login", "Failed", HttpStatusCode.BadRequest);
                }
                if (!id.Equals(theToken.UserId))
                {
                    return ResponseUtil.Error("Invalid verification token with user", "Failed", HttpStatusCode.BadRequest);
                }
                String verificationResult = await validateToken(token, id);

                if (verificationResult.Equals("Valid"))
                {
                    return ResponseUtil.GetObject(null, "Verification Email Successfully", HttpStatusCode.Created, null);
                }
                if (verificationResult.Equals("Token already expired"))
                {
                    return ResponseUtil.Error("Token already expired", "Verification Email Failed", HttpStatusCode.BadRequest);
                }
                
                return ResponseUtil.Error("Invalid verification token", "Invalid token", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }

        private async Task<string> validateToken(string theToken, int id)
        {
            VerificationToken token =  await _verificationTokenRepository.FindByTokenAsync(theToken);
            if (token == null || token.UserId != id)
            {
                return "Invalid verification token";
            }
            User user = token.User;
            
            DateTime ex = token.ExpirationTime;
            DateTime no = DateTime.Now;
            TimeSpan timeRemaining = ex - no;
            if (timeRemaining.TotalMinutes <= 0)
            {
                user.VerificationTokenId = null;
                user.VerificationToken = null;
                await _userRepository.UpdateAsync(user);
                await _verificationTokenRepository.DeleteAsync(token.Id);
                return "Token already expired";
            }
            else
            {
                user.IsEnabled = true;
                user.VerificationTokenId = null;
                user.VerificationToken = null;
                await _userRepository.UpdateAsync(user);
                await _verificationTokenRepository.DeleteAsync(token.Id);
                return "Valid";
            }
        }

        public async Task<ResponseDTO> ResetVerifyEmailAsync(string email, int id)
        {
            try
            {
                User user = await _userRepository.FindUserByEmailAsync(email);
                VerificationToken? verificationToken = await _verificationTokenRepository.FindByUserIdAsync(id);
                if (user.Id.Equals(id) && !user.IsEnabled && verificationToken == null)
                {
                    var sendEmail = await _emailService.SendEmail(email);
                    if (sendEmail.StatusCode.Equals(HttpStatusCode.BadRequest) )
                    {
                        return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                    }
                
                    var result = _mapper.Map<UpsertUserDTO>(user);

                    return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
                }
                else
                {
                    return ResponseUtil.Error("User does not exist", "Failed", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> SaveInfo(SignUp signUp)
        {
            try
            {
                User? user = await _userRepository.FindUserByEmailAsync(signUp.Email);
                if (user == null)
                {
                    return ResponseUtil.Error("Email not exist", "Failed", HttpStatusCode.BadRequest);
                }

                if (!user.IsEnabled)
                {
                    return ResponseUtil.Error("Please verify email before send password", "Failed", HttpStatusCode.BadRequest); 
                }
                user.Fullname = signUp.Fullname;
                user.PhoneNumber = signUp.PhoneNumber;
                user.Password = BCrypt.Net.BCrypt.HashPassword(signUp.Password);
                user.Address = signUp.Address;
                user.Gender = signUp.Gender;
                user.Role = Role.CUSTOMER;
                user.FcmToken = signUp.FcmToken;
                var result = _mapper.Map<UpsertUserDTO>(user);
                await _userRepository.UpdateAsync(user);
                return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
            
        }

        public async Task<ResponseDTO> SignIn(SignInRequest signInRequest)
        {
            try
            {
                User? user = await _userRepository.FindUserByEmailAsync(signInRequest.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(signInRequest.Password, user.Password))
                {
                    return ResponseUtil.Error("Email or Password not exist", "Failed", HttpStatusCode.BadRequest);
                }

                var jwt = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(user, new Dictionary<string, object>());
                
                JwtAuthenticationResponse jwtAuthResponse = new JwtAuthenticationResponse();
                UserDTO userDto = _mapper.Map<UserDTO>(user);
                
                jwtAuthResponse.UserDTO = userDto;
                jwtAuthResponse.Token = jwt;
                jwtAuthResponse.RefreshToken = refreshToken;
                
                return ResponseUtil.GetObject(jwtAuthResponse, "ok", HttpStatusCode.Created, null);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<ResponseDTO> SignInGoogle(String email)
        {
            try
            {
                User? userCheck = await _userRepository.FindUserByEmailAsync(email);
                User user = new User();
                if (userCheck == null)
                {
                    user.Email = email;
                    user.Role = Role.CUSTOMER;
                    user.IsEnabled = true;
                    await _userRepository.SaveAsync(user);
                }
                else
                {
                    user = userCheck;
                }

                var jwt = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(user, new Dictionary<string, object>());
                
                JwtAuthenticationResponse jwtAuthResponse = new JwtAuthenticationResponse();
                UserDTO userDto = _mapper.Map<UserDTO>(user);
                
                jwtAuthResponse.UserDTO = userDto;
                jwtAuthResponse.Token = jwt;
                jwtAuthResponse.RefreshToken = refreshToken;
                
                return ResponseUtil.GetObject(jwtAuthResponse, "ok", HttpStatusCode.Created, null);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }
        
        
        
        
}