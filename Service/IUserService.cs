using System.Security.Claims;
using BusinessObject;

namespace Service;

public interface IUserService
{
    Task<ClaimsPrincipal> LoadUserByUsernameAsync(string email);
    Task SaveUserVerificationTokenAsync(User user, string token);
    Task<string> ValidateTokenAsync(string theToken, long userId);
}