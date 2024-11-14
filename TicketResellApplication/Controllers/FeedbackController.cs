using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.ComponentModel.DataAnnotations;

namespace TicketResellApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("get-all-feedbacks")]
        public async Task<ResponseDTO> GetAllFeedbacks([FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _feedbackService.GetAllFeedbacks(page, limit);
        }

        [HttpGet("get-by-ticketid")]
        public async Task<ResponseDTO> GetFeedBacksByTicketId([FromQuery, Required] int ticketId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _feedbackService.GetFeedBacksByTicketId(ticketId, page, limit);
        }

        [HttpGet("get-by-postid")]
        public async Task<ResponseDTO> GetFeedBacksByPostId([FromQuery, Required] int postId, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _feedbackService.GetFeedBacksByPostId(postId, page, limit);
        }

        [HttpGet("get-by-feedbackid")]
        public async Task<ResponseDTO> GetFeedbackById([FromQuery, Required] int id)
        {
            return await _feedbackService.GetFeedbackById(id);
        }

        [HttpGet("get-by-userid")]
        public async Task<ResponseDTO> GetFeedbackByUserId([FromQuery, Required] int id, [FromQuery, Required] int page = 1,
            [FromQuery, Required] int limit = 10)
        {
            return await _feedbackService.GetFeedbackByUserId(id, page, limit);
        }

        [HttpPost("new")]
        public async Task<ResponseDTO> AddFeedback([FromBody] NewFeedback feedback)
        {
            return await _feedbackService.AddFeedback(feedback);
        }

        [HttpPut("remove")]
        public async Task<ResponseDTO> DeleteFeedback([FromQuery, Required] int id)
        {
            return await _feedbackService.DeleteFeedback(id);
        }

        [HttpPost("img")]
        public async Task<ResponseDTO> UpdateFeedbackImg([FromBody] List<String> imgList, [FromQuery, Required] int feedbackId)
        {
            return await _feedbackService.UploadImg(imgList, feedbackId);
        }

        [HttpGet("repu")]
        public async Task<ResponseDTO> UserReputation()
        {
            return await _feedbackService.UpdateUserReputation();
        }

    }
}
