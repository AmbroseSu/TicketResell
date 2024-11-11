using BusinessObject.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class PostElement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PostStatus Status { get; set; }
        public bool IsDeleted { get; set;}
        public DateTime CreatedDate { get; set; }
    }
}
