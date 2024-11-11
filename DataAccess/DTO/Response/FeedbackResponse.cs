using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class FeedbackResponse
    {
        public int Id { get; set; }        
        public int? Rating { get; set; }       
        public string? Context { get; set; }
        public bool IsDeleted { get; set; } 
        public int? TicketId { get; set; }
        public int? UserId { get; set; }
    }
}
