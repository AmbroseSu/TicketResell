using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

namespace Service;

public interface IAuthenticationService
{
    Task<ResponseDTO> CheckEmailAsync(string email);
    Task<ResponseDTO> VerifyEmailAsync(string token, int id);
    Task<ResponseDTO> ResetVerifyEmailAsync(string email, int id);
    Task<ResponseDTO> SaveInfo(SignUp signUp);
    Task<ResponseDTO> SignIn(SignInRequest signInRequest);
    Task<ResponseDTO> SignInGoogle(String email);
    Task<ResponseDTO> SignUpForStaff(SignUpForStaff signUpForStaff);
    Task<ResponseDTO> SaveInfoGoogle(SignUpGoogle signUpGoogle);
    Task<ResponseDTO> ChangePasswordAsync(ChangePassword changePassword);
    Task<ResponseDTO> CheckEmailForgotPasswordAsync(string email);
    Task<ResponseDTO> VerifyEmailForgotPasswordAsync(string token, int id);
    Task<ResponseDTO> ChangePasswordForgotPasswordAsync(string email, string newPassword);
    Task<ResponseDTO> ResetVerifyEmailForgotPasswordAsync(string email, int id);
}