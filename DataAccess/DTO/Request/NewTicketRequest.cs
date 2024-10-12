using BusinessObject.Enums;

namespace DataAccess.DTO.Request
{
    public class NewTicketRequest
    {
        public string? Name { get; set; }
        public float? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public List<string?> imgList { get; set; } 
        public int? CategoryId { get; set; }
    }
}
