using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class NewPostRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserId { get; set; }
        public int? TicketId { get; set; }

    }
}
