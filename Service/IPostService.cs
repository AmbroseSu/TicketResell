using DataAccess.DTO;
using DataAccess.DTO.Response;
using Service.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPostService
    {
       Task<ResponseDTO> CreatePost(PostDTO post, int TicketId);
       Task<ResponseDTO> EditPost(PostDTO post);
    }
}
