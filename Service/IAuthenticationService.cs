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
}