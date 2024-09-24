using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Chat
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<UserChat>? UserChats { get; set; }
    public List<Message>? Messages { get; set; }
    
}