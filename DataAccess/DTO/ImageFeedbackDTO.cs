using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ImageFeedbackDTO
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
