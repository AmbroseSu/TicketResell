using BusinessObject.Enums;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class TicketResponse
    {
        public TicketResponse(int id, string? ticketName, float? price, int? quantity, DateTime? expirationDate, string? venue, TicketStatus status, int? categoryId, string? categoryName, int postId, string? postTitle, string? postDescription, DateTime? createdDate, bool postStatus, int? userId, string email)
        {
            Id = id;
            TicketName = ticketName;
            Price = price;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            Venue = venue;
            Status = status;
            CategoryId = categoryId;
            CategoryName = categoryName;
            PostId = postId;
            PostTitle = postTitle;
            PostDescription = postDescription;
            CreatedDate = createdDate;
            PostStatus = postStatus;
            UserId = userId;
            Email = email;
        }

        public int Id { get; set; }
        public string? TicketName { get; set; }
        public float? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public TicketStatus Status { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int PostId { get; set; }
        public string? PostTitle { get; set; }
        public string? PostDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool PostStatus { get; set; }
        public int? UserId { get; set; }
        public string Email { get; set; }

        public List<ImageTicketDTO> imageTicketDTOs { get; set; }


    }
}
