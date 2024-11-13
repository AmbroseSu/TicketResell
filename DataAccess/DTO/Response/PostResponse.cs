using BusinessObject.enums;
using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class PostResponse
    {
        public PostResponse()
        {
        }

        public List<PostElement> PostElements { get; set; }
        public int TicketId { get; set; }
        public string? TicketName { get; set; }
        public float? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public TicketStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public List<ImageTicketDTO> imageTicketDTOs { get; set; }
        public List<FeedbackTicketElement>? feedbackDTOs { get; set; }

    }
}
