using BusinessObject.enums;
using BusinessObject.Enums;

namespace DataAccess.DTO;

public class UserDTO
{
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
    public string Fcm { get; set; }
    public bool IsDelete { get; set; }
    
    public List<int> DeliveryIds { get; set; }
    public List<int> OrdersIds { get; set; }
    public List<int> ReturnsIds { get; set; }
    public List<int> MessagesIds { get; set; }
    public List<int> UserChatsIds { get; set; }
    public int CartId { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(int id, string fullname, string address, string email, string phone, string status, Gender gender, Role role, string fcm, bool isDelete, List<int> deliveryIds, List<int> ordersIds, List<int> returnsIds, List<int> messagesIds, List<int> userChatsIds, int cartId)
    {
        Id = id;
        Fullname = fullname;
        Address = address;
        Email = email;
        Phone = phone;
        Status = status;
        Gender = gender;
        Role = role;
        Fcm = fcm;
        IsDelete = isDelete;
        DeliveryIds = deliveryIds;
        OrdersIds = ordersIds;
        ReturnsIds = returnsIds;
        MessagesIds = messagesIds;
        UserChatsIds = userChatsIds;
        CartId = cartId;
    }
}