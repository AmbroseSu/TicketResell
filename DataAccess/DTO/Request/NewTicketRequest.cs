using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class NewTicketRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Name { get; set; }
        public float? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
