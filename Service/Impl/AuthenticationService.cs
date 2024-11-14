using System.Net;
using System.Text.RegularExpressions;
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
                if (!IsValidEmail(email))
                {
                    return ResponseUtil.Error("Invalid email format", "Failed", HttpStatusCode.BadRequest);
                }
                if (await _userRepository.ExistsByEmailAsync(email) && (await _userRepository.FindUserByEmailAsync(email)).IsEnabled)
                {
                    User userCheck = await _userRepository.FindUserByEmailAsync(email);
                    int userId = userCheck.Id;
                    userCheck.VerificationTokenId = null;
                    userCheck.VerificationToken = null;
                    await _userRepository.UpdateAsync(userCheck);
                    var verification = await _verificationTokenRepository.FindByUserIdAsync(userId);
                    if (verification != null)
                    {
                        await _verificationTokenRepository.DeleteAsync(verification.Id);
                    }
                    return ResponseUtil.GetObject(null, "Please Sign Up", HttpStatusCode.OK, 0);;
                }

                if (await _userRepository.ExistsByEmailAsync(email) &&
                    !(await _userRepository.FindUserByEmailAsync(email)).IsEnabled)
                {
                    User userCheck = await _userRepository.FindUserByEmailAsync(email);
                    int userId = userCheck.Id;
                    userCheck.VerificationTokenId = null;
                    userCheck.VerificationToken = null;
                    await _userRepository.UpdateAsync(userCheck);
                    var verification = await _verificationTokenRepository.FindByUserIdAsync(userId);
                    if (verification != null)
                    {
                        await _verificationTokenRepository.DeleteAsync(verification.Id);
                    }
                    var sendEmail1 = await _emailService.SendEmail(email);
                    if (sendEmail1.StatusCode.Equals(HttpStatusCode.BadRequest) )
                    {
                        return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                    }
                    var result1 = _mapper.Map<UpsertUserDTO>(userCheck);

                    return ResponseUtil.GetObject(result1, "ok", HttpStatusCode.Created, 0);
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

                return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
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
                    return ResponseUtil.GetObject(null, "Verification Email Successfully", HttpStatusCode.Created, 0);
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
            DateTime no = DateTime.UtcNow;
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
                    int userId = user.Id;
                    user.VerificationTokenId = null;
                    user.VerificationToken = null;
                    await _userRepository.UpdateAsync(user);
                    var verification = await _verificationTokenRepository.FindByUserIdAsync(userId);
                    if (verification != null)
                    {
                        await _verificationTokenRepository.DeleteAsync(verification.Id);
                    }
                    var sendEmail = await _emailService.SendEmail(email);
                    if (sendEmail.StatusCode.Equals(HttpStatusCode.BadRequest) )
                    {
                        return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                    }
                
                    var result = _mapper.Map<UpsertUserDTO>(user);

                    return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
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
                if (!IsValidPassword(signUp.Password))
                {
                    return ResponseUtil.Error("Invalid password format", "Failed", HttpStatusCode.BadRequest);
                }
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
                user.Image = signUp.Image;
                user.CreatedAt = DateTime.UtcNow;
                user.Role = Role.CUSTOMER;
                user.FcmToken = signUp.FcmToken;
                var result = _mapper.Map<UpsertUserDTO>(user);
                await _userRepository.UpdateAsync(user);
                return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
            
        }
        private bool IsValidPassword(string password)
        {
            // Mật khẩu phải có ít nhất 8 ký tự
            if (password.Length < 8)
            {
                return false;
            }

            // Biểu thức chính quy kiểm tra ít nhất 1 ký tự đặc biệt, 1 chữ hoa, 1 chữ thường và 1 số
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            return passwordRegex.IsMatch(password);
        }

        public async Task<ResponseDTO> SignIn(SignInRequest signInRequest)
        {
            try
            {
                if (!IsValidEmail(signInRequest.Email))
                {
                    return ResponseUtil.Error("Invalid email format", "Failed", HttpStatusCode.BadRequest);
                }
                User? user = await _userRepository.FindUserByEmailAsync(signInRequest.Email.ToLower());
                if (user == null || !BCrypt.Net.BCrypt.Verify(signInRequest.Password, user.Password))
                {
                    return ResponseUtil.Error("Email or Password not exist", "Failed", HttpStatusCode.BadRequest);
                }

                if (user.IsDeleted == true)
                {
                    return ResponseUtil.Error("User is deleted", "Failed", HttpStatusCode.BadRequest);
                }

                if (!signInRequest.FcmToken.Equals(user.FcmToken) && !signInRequest.FcmToken.Equals("string"))
                {
                    user.FcmToken = signInRequest.FcmToken;
                    await _userRepository.UpdateAsync(user);
                }

                var jwt = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(user, new Dictionary<string, object>());
                
                JwtAuthenticationResponse jwtAuthResponse = new JwtAuthenticationResponse();
                UserDTO userDto = _mapper.Map<UserDTO>(user);
                
                jwtAuthResponse.UserDTO = userDto;
                jwtAuthResponse.Token = jwt;
                jwtAuthResponse.RefreshToken = refreshToken;
                
                return ResponseUtil.GetObject(jwtAuthResponse, "ok", HttpStatusCode.Created, 0);
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
                    user.IsDeleted = false;
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
                
                return ResponseUtil.GetObject(jwtAuthResponse, "ok", HttpStatusCode.Created, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> SignUpForStaff(SignUpForStaff signUpForStaff)
        {
            try
            {
                if (!IsValidPassword(signUpForStaff.Password))
                {
                    return ResponseUtil.Error("Invalid password format", "Failed", HttpStatusCode.BadRequest);
                }
                if (!IsValidEmail(signUpForStaff.Email))
                {
                    return ResponseUtil.Error("Invalid email format", "Failed", HttpStatusCode.BadRequest);
                }
                User? user = await _userRepository.FindUserByEmailAsync(signUpForStaff.Email);
                if (user != null)
                {
                    return ResponseUtil.Error("Emailexist", "Failed", HttpStatusCode.BadRequest);
                }
                else
                {
                    User userSignUp = new User();
                    userSignUp.Email = signUpForStaff.Email;
                    userSignUp.Fullname = signUpForStaff.Fullname;
                    userSignUp.PhoneNumber = signUpForStaff.PhoneNumber;
                    userSignUp.Password = BCrypt.Net.BCrypt.HashPassword(signUpForStaff.Password);
                    userSignUp.Address = signUpForStaff.Address;
                    userSignUp.Gender = signUpForStaff.Gender;
                    userSignUp.Image = signUpForStaff.Image;
                    userSignUp.CreatedAt = DateTime.UtcNow;
                    userSignUp.IsEnabled = true;
                    userSignUp.Role = Role.STAFF;
                    userSignUp.FcmToken = null;
                    await _userRepository.SaveAsync(userSignUp);
                    var result = _mapper.Map<UserDTO>(userSignUp);
                    return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
                }
                
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }
        
        
        public async Task<ResponseDTO> SaveInfoGoogle(SignUpGoogle signUpGoogle)
        {
            try
            {
                User? user = await _userRepository.FindUserByEmailAsync(signUpGoogle.Email);
                if (user == null)
                {
                    return ResponseUtil.Error("Email not exist", "Failed", HttpStatusCode.BadRequest);
                }

                if (user.Fullname == null &&
                    user.PhoneNumber == null &&
                    user.Address == null)
                {
                    user.Fullname = signUpGoogle.Fullname;
                    user.PhoneNumber = signUpGoogle.PhoneNumber;
                    user.Address = signUpGoogle.Address;
                    user.Gender = signUpGoogle.Gender;
                    user.CreatedAt = DateTime.UtcNow;
                    user.Image = signUpGoogle.Image;
                    user.FcmToken = signUpGoogle.FcmToken;
                }
                
                var result = _mapper.Map<UpsertUserDTO>(user);
                await _userRepository.UpdateAsync(user);
                return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
            
        }
        
        public async Task<ResponseDTO> ChangePasswordAsync(ChangePassword changePassword)
        {
            try
            {
                if (!IsValidPassword(changePassword.NewPassword))
                {
                    return ResponseUtil.Error("Invalid password format", "Failed", HttpStatusCode.BadRequest);
                }
                var user = await _userRepository.FindUserByEmailAsync(changePassword.Email);
                
                if (user == null)
                {
                    return ResponseUtil.Error("User does not exist", "Failed", HttpStatusCode.BadRequest);
                }
                
                if (!BCrypt.Net.BCrypt.Verify(changePassword.Password, user.Password))
                {
                    return ResponseUtil.Error("Current password is incorrect", "Failed", HttpStatusCode.BadRequest);
                }

                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
        

                user.Password = hashedNewPassword;
        

                await _userRepository.UpdateAsync(user);
                var result = _mapper.Map<UpsertUserDTO>(user);
                return ResponseUtil.GetObject(result, "Password changed successfully", HttpStatusCode.OK, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.BadRequest);
            }
        }
        
        
        public async Task<ResponseDTO> CheckEmailForgotPasswordAsync(string email)
        {
            try
            {
                if (!IsValidEmail(email))
                {
                    return ResponseUtil.Error("Invalid email format", "Failed", HttpStatusCode.BadRequest);
                }
                if (await _userRepository.ExistsByEmailAsync(email) && (await _userRepository.FindUserByEmailAsync(email)).IsEnabled)
                {
                    User userCheck = await _userRepository.FindUserByEmailAsync(email);
                    int userId = userCheck.Id;
                    userCheck.VerificationTokenId = null;
                    userCheck.VerificationToken = null;
                    await _userRepository.UpdateAsync(userCheck);
                    var verification = await _verificationTokenRepository.FindByUserIdAsync(userId);
                    if (verification != null)
                    {
                        await _verificationTokenRepository.DeleteAsync(verification.Id);
                    }
                    var sendEmail1 = await _emailService.SendEmail(email);
                    if (sendEmail1.StatusCode.Equals(HttpStatusCode.BadRequest) )
                    {
                        return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                    }
                    var result1 = _mapper.Map<UpsertUserDTO>(userCheck);

                    return ResponseUtil.GetObject(result1, "ok", HttpStatusCode.Created, 0);
                }
                return ResponseUtil.Error("Account not exists", "Failed", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<ResponseDTO> VerifyEmailForgotPasswordAsync(string token, int id)
        {
            try
            {
                VerificationToken theToken = await _verificationTokenRepository.FindByTokenAsync(token);

                if (theToken == null)
                {
                    return ResponseUtil.Error("Token not exist", "Failed", HttpStatusCode.BadRequest);
                }
                if (!id.Equals(theToken.UserId))
                {
                    return ResponseUtil.Error("Invalid verification token with user", "Failed", HttpStatusCode.BadRequest);
                }
                String verificationResult = await validateTokenForgotPassword(token, id);

                if (verificationResult.Equals("Valid"))
                {
                    return ResponseUtil.GetObject(null, "Verification Email Successfully", HttpStatusCode.Created, 0);
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
        
        public async Task<ResponseDTO> ChangePasswordForgotPasswordAsync(ChangePasswordForgot changePasswordForgot)
        {
            try
            {
                if (!changePasswordForgot.Status)
                {
                    return ResponseUtil.Error("Please verify email and check OTP", "Failed", HttpStatusCode.BadRequest);
                }
                if (!IsValidPassword(changePasswordForgot.NewPassword))
                {
                    return ResponseUtil.Error("Invalid password format", "Failed", HttpStatusCode.BadRequest);
                }
                var user = await _userRepository.FindUserByEmailAsync(changePasswordForgot.Email.ToLower());
                
                if (user == null)
                {
                    return ResponseUtil.Error("User does not exist", "Failed", HttpStatusCode.BadRequest);
                }

                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordForgot.NewPassword);
        

                user.Password = hashedNewPassword;
        

                await _userRepository.UpdateAsync(user);
                var result = _mapper.Map<UpsertUserDTO>(user);
                return ResponseUtil.GetObject(result, "Password changed successfully", HttpStatusCode.OK, 0);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.BadRequest);
            }
        }
        
        public async Task<ResponseDTO> ResetVerifyEmailForgotPasswordAsync(string email, int id)
        {
            try
            {
                User user = await _userRepository.FindUserByEmailAsync(email);
                VerificationToken? verificationToken = await _verificationTokenRepository.FindByUserIdAsync(id);
                if (user.Id.Equals(id) && verificationToken == null)
                {
                    var sendEmail = await _emailService.SendEmail(email);
                    if (sendEmail.StatusCode.Equals(HttpStatusCode.BadRequest) )
                    {
                        return ResponseUtil.Error("Can't Send", "Failed", HttpStatusCode.BadRequest);
                    }
                
                    var result = _mapper.Map<UpsertUserDTO>(user);

                    return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, 0);
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
        
        
        private async Task<string> validateTokenForgotPassword(string theToken, int id)
        {
            VerificationToken token =  await _verificationTokenRepository.FindByTokenAsync(theToken);
            if (token == null || token.UserId != id)
            {
                return "Invalid verification token";
            }
            User user = token.User;
            
            DateTime ex = token.ExpirationTime;
            DateTime no = DateTime.UtcNow;
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
                await _verificationTokenRepository.DeleteAsync(token.Id);
                return "Valid";
            }
        }
        
        
        
}