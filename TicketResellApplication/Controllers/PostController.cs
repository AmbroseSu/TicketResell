using DataAccess.DTO;
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

        [HttpPost("new-post")]
        public async Task<ResponseDTO> CreatePostAsync([FromBody] PostDTO postDTO, [FromQuery] int ticketId)
        {
            return await _postService.CreatePost(postDTO, ticketId);
        }

        [HttpPut("edit-post")]
        public async Task<ResponseDTO> UpdatePostAsync([FromBody] PostDTO postDTO)
        {
            return await _postService.EditPost(postDTO);
        }

        //[HttpGet("get-posts")]
        //public async Task<ResponseDTO> GetPostsAsync()
        //{
        //    return await _postService.GetPosts();
        //}





       
    }
}
