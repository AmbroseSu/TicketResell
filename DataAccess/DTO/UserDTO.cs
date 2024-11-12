using BusinessObject.enums;
using BusinessObject.Enums;

namespace DataAccess.DTO;

public class UserDTO
{
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public Gender Gender { get; set; }
    public string Image { get; set; }
    public Role Role { get; set; }
    public string FcmToken { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int> DeliveryIds { get; set; }
    public List<int> OrdersIds { get; set; }
    public List<int> ReturnsIds { get; set; }
    public List<int> MessagesIds { get; set; }
    public List<int> UserChatsIds { get; set; }
    public int CartId { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(int id, string fullname, string address, string email, string phoneNumber, string status, Gender gender, string image, Role role, string fcmToken, bool isDeleted, List<int> deliveryIds, List<int> ordersIds, List<int> returnsIds, List<int> messagesIds, List<int> userChatsIds, int cartId)
    {
        Id = id;
        Fullname = fullname;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        Status = status;
        Gender = gender;
        Image = image;
        Role = role;
        FcmToken = fcmToken;
        IsDeleted = isDeleted;
        DeliveryIds = deliveryIds;
        OrdersIds = ordersIds;
        ReturnsIds = returnsIds;
        MessagesIds = messagesIds;
        UserChatsIds = userChatsIds;
        CartId = cartId;
    }
}