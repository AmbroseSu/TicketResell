using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace TicketResellApplication.Controllers
{
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
        public async Task<ResponseDTO> CreatePostAsync([FromBody] NewPostRequest post, 
            [FromQuery] int ticketId,
            [FromQuery] int userId)
        {
            return await _postService.CreatePost(post, ticketId, userId);
        }

        [HttpPut("edit")]
        public async Task<ResponseDTO> UpdatePostAsync([FromQuery] string description, [FromQuery] int ticketId)
        {
            return await _postService.EditPost(ticketId, description);
        }

        [HttpPut("remove")]
        public async Task<ResponseDTO> DeletePost([FromQuery] int ticketId)
        {
            return await _postService.DeletePost(ticketId);
        }

        [HttpGet("current-post")]
        public async Task<ResponseDTO> GetCurrentPostsAsync([FromQuery] int page = 1,
         [FromQuery] int limit = 1)
        {
            return await _postService.getCurrentPosts(page, limit);
        }

        [HttpGet("get-lists")]
        public async Task<ResponseDTO> GetAllPosts([FromQuery] int page = 1,
         [FromQuery] int limit = 1)
        {
            return await _postService.getAllPosts(page,limit);
        }

        [HttpGet("get")]
        public async Task<ResponseDTO> GetPostsAsync(int id)
        {
            return await _postService.GetPost(id);
        }
    }
}
