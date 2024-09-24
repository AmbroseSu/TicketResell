namespace DataAccess.DTO;

public class PlatformFeeDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? TransactionIds { get; set; }
}