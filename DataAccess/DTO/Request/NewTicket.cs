using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class NewTicket
    {

        public string? Name { get; set; }
        [Range(20000, float.MaxValue, ErrorMessage = "Price must be between 20000 - float max value")]
        public float? Price { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 - 100")]
        public int? Quantity { get; set; }
        public String ExpirationDate { get; set; }
        public string? Venue { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
