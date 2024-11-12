using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class NewPostRequest
    {
        [StringLength(1000, ErrorMessage = "Name must be less than 1000 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z 1-9]+$", ErrorMessage = "Title must contain only letters or number")]
        public string Title { get; set; }
        [StringLength(3000, ErrorMessage = "Name must be less than 3000 characters")]
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z 1-9]+$", ErrorMessage = "Description must contain only letters or number")]
        public string Description { get; set; }
        public int ticketId { get; set; }
    }
}
