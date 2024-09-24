using System.Security.Claims;
using BusinessObject;

namespace Service;

public interface IJwtService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(User user, Dictionary<string, object> extraClaims);
    string ExtractUserName(string token);
    string ExtractRoles(string token);
    bool IsTokenValid(string token, ClaimsPrincipal userPrincipal);
}