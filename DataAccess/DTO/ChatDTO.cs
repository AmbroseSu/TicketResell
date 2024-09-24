namespace DataAccess.DTO;

public class ChatDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? UserChatIds { get; set; }
    public List<int>? MessageIds { get; set; }
}