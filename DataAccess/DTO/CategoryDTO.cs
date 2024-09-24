namespace DataAccess.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? TicketIds { get; set; }
}