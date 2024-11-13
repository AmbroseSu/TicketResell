using BusinessObject.enums;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.ComponentModel.DataAnnotations;

namespace TicketResellApplication.Controllers
{
    [EnableCors("AllowReactApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpPost("new")]
        public async Task<ResponseDTO> CreatePost([FromBody] NewPostRequest post
            )
        {
            return await _postService.CreatePost(post);
        }

        [HttpPut("remove")]
        public async Task<ResponseDTO> DeletePost([FromQuery, Required] int postId)
        {
            return await _postService.DeletePost(postId);
        }

        [HttpPut("manager-action")]
        public async Task<ResponseDTO> UpdateStatus([FromQuery, Required] int postId,
          [FromQuery, Required] PostStatus status)
        {
            return await _postService.UpdateStatus(postId, status);
        }

        [HttpGet("get-lists")]
        public async Task<ResponseDTO> GetAllPosts(
            [FromQuery] PostStatus? status,
         [FromQuery] string searchTerm = "",
            [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10)
        {
            return await _postService.GetAllPosts(page, limit, status, searchTerm);
        }

        [HttpGet("get")]
        public async Task<ResponseDTO> GetPostByPostId([FromQuery, Required] int id)
        {
            return await _postService.GetPostByPostId(id);
        }

        [HttpGet("get-by-ticket")]
        public async Task<ResponseDTO> GetPostByTicketId([FromQuery, Required] int id,
            [FromQuery, Required] int page = 1,
         [FromQuery, Required] int limit = 10)
        {
            return await _postService.GetPostByTicketId(id, page, limit);
        }

        [HttpGet("get-by-user")]
        public async Task<ResponseDTO> GetPostByUserId(
            [FromQuery] PostStatus? status,
            [FromQuery, Required] int id,
           [FromQuery, Required] int page = 1,
        [FromQuery, Required] int limit = 10)
        {
            return await _postService.GetPostByUserId(id, status, page, limit);
        }
    }
}
