using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class UserChat
{
    [Key, Column(Order = 0)]
    public int ChatId { get; set; }
    [Key, Column(Order = 1)]
    public int UserId { get; set; }
    
    [ForeignKey(("ChatId"))]
    public Chat? Chat { get; set; }
    [ForeignKey(("UserId"))]
    public User? User { get; set; }
}