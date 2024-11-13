namespace DataAccess.DTO.Request;

public class ChangePassword
{
    public String Email { get; set; }
    public String Password { get; set; }
    public String NewPassword { get; set; }
}