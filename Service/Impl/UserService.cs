using System.Security.Claims;
using BusinessObject;
using Repository;

namespace Service.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationTokenRepository _tokenRepository;

    public UserService(
        IUserRepository userRepository,
        IVerificationTokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
    }

    public async Task<ClaimsPrincipal> LoadUserByUsernameAsync(string email)
    {
        var user = await _userRepository.FindByLoginAsync(email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
            // Thêm các claim khác nếu cần
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CustomAuthType");
        return new ClaimsPrincipal(claimsIdentity);
    }

    public async Task SaveUserVerificationTokenAsync(User user, string token)
    {
        var verificationToken = new VerificationToken(token, user.Id);
        await _tokenRepository.SaveAsync(verificationToken);
    }

    public async Task<string> ValidateTokenAsync(string theToken, long userId)
    {
        var token = await _tokenRepository.FindByTokenAsync(theToken);
        if (token == null || token.User.Id != userId)
        {
            return "Invalid verification token";
        }

        var user = token.User;
        var currentTime = DateTime.Now;
        var tokenExpirationTime = token.ExpirationTime;

        if (tokenExpirationTime <= currentTime)
        {
            await _tokenRepository.DeleteAsync(token.Id);
            return "Token already expired";
        }

        user.IsEnabled = true; // Assuming user has an IsEnabled property
        await _userRepository.SaveAsync(user);
        await _tokenRepository.DeleteAsync(token.Id);
        return "Valid";
    }
}