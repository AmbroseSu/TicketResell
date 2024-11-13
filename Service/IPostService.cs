using BusinessObject.enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
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
        Task<ResponseDTO> CreatePost(NewPostRequest post);
        //Task<ResponseDTO> EditPost(int id, string description);
        //Task<ResponseDTO> DeletePost(int id);
        //Task<ResponseDTO> getCurrentPosts(int page, int limit);
        Task<ResponseDTO> getAllPosts(int page, int limit, PostStatus? status, String? searchTerm);
        //Task<ResponseDTO> GetPost(int id);
        //Task<ResponseDTO> PostVerify(int id);
    }
}
