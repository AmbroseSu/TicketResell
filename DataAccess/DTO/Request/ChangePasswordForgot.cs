namespace DataAccess.DTO.Request;

public class ChangePasswordForgot
{
    public String Email { get; set; }
    public String NewPassword { get; set; }
    public bool Status { get; set; }
}