using BusinessObject.enums;
using BusinessObject.Enums;

namespace DataAccess.DTO;

public class UpsertUserDTO
{
    public int UserId { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }

    public UpsertUserDTO()
    {
    }

    public UpsertUserDTO(int userId, string fullname, string email, string address, string phone, Gender gender, Role role)
    {
        UserId = userId;
        Fullname = fullname;
        Email = email;
        Address = address;
        Phone = phone;
        Gender = gender;
        Role = role;
    }
}