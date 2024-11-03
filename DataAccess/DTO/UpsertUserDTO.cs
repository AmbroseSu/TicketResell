using BusinessObject.enums;
using BusinessObject.Enums;

namespace DataAccess.DTO;

public class UpsertUserDTO
{
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Image { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }

    public UpsertUserDTO()
    {
    }

    public UpsertUserDTO(int id, string fullname, string email, string address, string phoneNumber, string image, Gender gender, Role role)
    {
        Id = id;
        Fullname = fullname;
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
        Image = image;
        Gender = gender;
        Role = role;
    }
}