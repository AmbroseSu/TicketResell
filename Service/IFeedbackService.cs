using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IFeedbackService
    {
        Task<ResponseDTO> GetAllFeedbacks(int page, int limit);
        Task<ResponseDTO> GetFeedBacksByTicketId(int ticketId, int page, int limit);
        Task<ResponseDTO> GetFeedBacksByPostId(int postId, int page, int limit);
        Task<ResponseDTO> GetFeedbackById(int id);
        Task<ResponseDTO> GetFeedbackByUserId(int id, int page, int limit);
        Task<ResponseDTO> AddFeedback(NewFeedback feedback);
        Task<ResponseDTO> DeleteFeedback(int id);
        Task<ResponseDTO> UploadImg(List<String> imgs, int feedbackId);
    }
}
