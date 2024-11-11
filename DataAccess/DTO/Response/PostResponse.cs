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

        public PostResponse(List<PostElement> postElements, string? ticketName, float? ticketPrice, int? ticketQuantity, DateTime? ticketExpirationDate, string? ticketVenue, TicketStatus ticketStatus, bool isDeleted, int categoryId, string categoryName, int userId, string email)
        {
            PostElements = postElements;
            TicketName = ticketName;
            TicketPrice = ticketPrice;
            TicketQuantity = ticketQuantity;
            TicketExpirationDate = ticketExpirationDate;
            TicketVenue = ticketVenue;
            TicketStatus = ticketStatus;
            IsDeleted = isDeleted;
            CategoryId = categoryId;
            CategoryName = categoryName;
            UserId = userId;
            Email = email;
        }

        public List<PostElement> PostElements { get; set; }
        public string? TicketName { get; set; }
        public float? TicketPrice { get; set; }
        public int? TicketQuantity { get; set; }
        public DateTime? TicketExpirationDate { get; set; }
        public string? TicketVenue { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public List<ImageTicketDTO> imageTicketDTOs { get; set; }

    }
}
