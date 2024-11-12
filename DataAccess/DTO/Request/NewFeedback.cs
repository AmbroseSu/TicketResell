using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class NewFeedback
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [StringLength(1000, ErrorMessage = "Name must be less than 1000 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z 1-9]+$", ErrorMessage = "Context must contain only letters or number")]
        public string Context { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
    }
}
