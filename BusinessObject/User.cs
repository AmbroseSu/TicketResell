using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.enums;
using BusinessObject.Enums;

namespace BusinessObject;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string? Fullname { get; set; }
    [StringLength(500)]
    public string? PhoneNumber { get; set; }
    [StringLength(500)]
    public string? Email { get; set; }
    [StringLength(2000)]
    public string? Address { get; set; }
    [StringLength(500)]
    public string? Password { get; set; }
    [StringLength(1000)]
    public string? Image { get; set; }
    public Role Role { get; set; }
    public Gender Gender { get; set; }
    [StringLength(2000)]
    public string? FcmToken { get; set; }
    public DateTime? PostTime { get; set; }
    public int? Points { get; set; }
    public bool Status { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsEnabled { get; set; } = false;
    
    public int? CartId { get; set; }
    public int? VerificationTokenId { get; set; }
    public List<Transaction>? Transactions { get; set; }
    public List<Feedback>? Feedbacks { get; set; }
    public List<Address>? Addresses { get; set; }
    public List<Post>? Posts { get; set; }
    public List<TicketRequest>? TicketRequests { get; set; }
    public List<Order>? Orders { get; set; }
    public List<Message>? Messages { get; set; }
    public List<UserChat>? UserChats { get; set; }
    public Cart? Cart { get; set; }
    public VerificationToken? VerificationToken { get; set; }
    
    
    


}