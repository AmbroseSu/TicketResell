using BusinessObject.enums;

namespace DataAccess.DTO.Request;

public class SignUpForStaff
{
    public string Email { get; set; }
    public string Fullname { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public string Image {get; set;}
    public Gender Gender { get; set; }
}