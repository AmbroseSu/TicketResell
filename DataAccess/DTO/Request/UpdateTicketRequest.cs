using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class updateTicketRequest
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public string? Venue { get; set; }
        public TicketStatus Status { get; set; }
    }
}
