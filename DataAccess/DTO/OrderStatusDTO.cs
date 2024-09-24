namespace DataAccess.DTO;

public class OrderStatusDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    
    public int OrderId { get; set; }
}