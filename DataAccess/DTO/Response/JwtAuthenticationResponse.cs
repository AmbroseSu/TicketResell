namespace DataAccess.DTO.Response;

public class JwtAuthenticationResponse
{
    public UserDTO UserDTO { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}