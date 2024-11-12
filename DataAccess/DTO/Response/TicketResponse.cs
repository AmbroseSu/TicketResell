using BusinessObject.Enums;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.enums;

namespace DataAccess.DTO.Response
{
    public class TicketResponse
    {
        public TicketResponse()
        {
        }

        public int Id { get; set; }
        public string? TicketName { get; set; }
        public float? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public TicketStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? PostId { get; set; }
        public string? PostTitle { get; set; }
        public string? PostDescription { get; set; }
        public PostStatus? CurrentPostStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }
        public string Email { get; set; }
        public List<ImageTicketDTO> imageTicketDTOs { get; set; }
        public List<FeedbackTicketElement>? feedbackDTOs { get; set; }


    }
}
