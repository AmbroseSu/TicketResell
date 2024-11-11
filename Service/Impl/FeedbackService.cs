using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Repository.Impl;
using Service.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public FeedbackService(ITicketRepository ticketRepository, IFeedbackRepository feedbackRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _feedbackRepository = feedbackRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<ResponseDTO> AddFeedback(FeedbackDTO feedback)
        {
            User? user = await _userRepository.FindUserByIdAsync(feedback.UserId);

            if (user == null)
            {
                String msg = "User not found";

                if (user.IsDeleted)
                {
                    msg = "User is deleted";
                }
                else if (!user.IsEnabled)
                {
                    msg = "User is not active";
                }
                return ResponseUtil.Error("Request fails", msg, HttpStatusCode.BadRequest);
            }

            //Kiểm tra xem item
            Ticket? ticket = (await _ticketRepository.Find(c => c.Id == feedback.TicketId)).SingleOrDefault();


            if (ticket == null)
            {
                String msg = "Ticket not found";

                return ResponseUtil.Error("Request fails", msg, HttpStatusCode.BadRequest);
            }

            //kiểm tra xem user có đặt item ko
            Order? order = (await _orderRepository.Find(c => c.TicketId == ticket.Id)).SingleOrDefault();

            if (order == null)
            {
                String msg = "Order not found to be feedback";
                //Kiểm tra order status là đã giao dịch hoàn tất chưa?

                return ResponseUtil.Error("Request fails", msg, HttpStatusCode.BadRequest);
            }

            //post có tồn tại ko
            Post? post = (await _postRepository.Find(c => c.TicketId == ticket.Id)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found", HttpStatusCode.BadRequest);
            }

            //Kiểm tra xem user này đã feedback chưa
            Feedback? feedbackExist = (await _feedbackRepository.Find(c => c.UserId == feedback.UserId && c.TicketId == feedback.TicketId)).SingleOrDefault();

            if (feedbackExist != null)
            {
                return ResponseUtil.Error("Request fails", "User has already feedback", HttpStatusCode.BadRequest);
            }

            Feedback result = _mapper.Map<Feedback>(feedback);
            await _feedbackRepository.SaveAsync(result);

            return ResponseUtil.GetObject(result, "Feedback added successfully", HttpStatusCode.Created, 0);
        }

        public async Task<ResponseDTO> DeleteFeedback(int id)
        {
            Feedback? feedback = (await _feedbackRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (feedback == null)
            {
                String msg = "Feedback not found";

                if (feedback.IsDeleted)
                {
                    msg = "Feedback is already deleted";
                }
                return ResponseUtil.Error("Request fails", msg, HttpStatusCode.NotFound);
            }

            return ResponseUtil.GetObject(feedback, "Feedback deleted successfully", HttpStatusCode.OK, 0);
        }

        public async Task<ResponseDTO> GetAllFeedbacks(int page, int limit)
        {
            IEnumerable<Feedback> feedbacks = await _feedbackRepository.GetAllAsync();

            if (feedbacks == null)
            {
                return ResponseUtil.GetCollection(null, "No feedbacks found", HttpStatusCode.OK, 0, page, limit, 0);
            }

            return await getListFeedbackToResponse(feedbacks.ToList(), page, limit);

        }

        public async Task<ResponseDTO> GetFeedbackById(int id)
        {
            Feedback? feedback = (await _feedbackRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (feedback == null)
            {
                return ResponseUtil.Error("Request fails", "Feedback not found", HttpStatusCode.BadRequest);
            }

            return await GetFeedbackToResponse(feedback);
        }

        public async Task<ResponseDTO> GetFeedBacksByPostId(int postId, int page, int limit)
        {
            Post? post = (await _postRepository.Find(c => c.Id == postId)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found", HttpStatusCode.BadRequest);
            }

            Ticket? ticket = (await _ticketRepository.Find(c => c.Id == post.TicketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found", HttpStatusCode.BadRequest);
            }

            return await GetFeedBacksByTicketId(ticket.Id, page, limit);

        }

        public async Task<ResponseDTO> GetFeedBacksByTicketId(int ticketId, int page, int limit)
        {
            IEnumerable<Feedback> feedbacks = await _feedbackRepository.Find(c => c.TicketId == ticketId);

            if (feedbacks == null)
            {
                return ResponseUtil.GetCollection(null, "No feedbacks found", HttpStatusCode.OK, 0, page, limit, 0);
            }

            return await getListFeedbackToResponse(feedbacks.ToList(), page, limit);
        }

        private async Task<ResponseDTO> getListFeedbackToResponse(List<Feedback?> result, int page, int limit)
        {
            List<FeedbackResponse?> responseData = new List<FeedbackResponse?>();

            //Duyệt qua list lấy item info tương ứng

            foreach (Feedback item in result)
            {
                User? user = await _userRepository.FindUserByIdAsync(item.UserId);

                if (user == null)
                {
                    ResponseUtil.Error("Request fails", "User not found", HttpStatusCode.BadRequest);
                }

                Ticket? ticket = (await _ticketRepository.Find(c => c.Id == item.TicketId)).SingleOrDefault();

                if (ticket == null)
                {
                    ResponseUtil.Error("Request fails", "Ticket not found", HttpStatusCode.BadRequest);
                }

                Post? post = (await _postRepository.Find(c => c.TicketId == item.TicketId)).SingleOrDefault();

                if (post == null)
                {
                    ResponseUtil.Error("Request fails", "Post not found", HttpStatusCode.BadRequest);
                }

                FeedbackResponse feedback = _mapper.Map<FeedbackResponse>(item);
                feedback.PostId = post.Id;
                responseData.Add(feedback);
            }

            List<FeedbackResponse?> data = responseData.Skip((page - 1) * limit).Take(limit).ToList();
            return ResponseUtil.GetCollection(data, "All feedbacks retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        }

        private async Task<ResponseDTO> GetFeedbackToResponse(Feedback feedback)
        {
            User? user = await _userRepository.FindUserByIdAsync(feedback.UserId);

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found", HttpStatusCode.BadRequest);
            }

            Ticket? ticket = (await _ticketRepository.Find(c => c.Id == feedback.TicketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found", HttpStatusCode.BadRequest);
            }

            Post? post = (await _postRepository.Find(c => c.TicketId == feedback.TicketId)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found", HttpStatusCode.BadRequest);
            }

            FeedbackResponse data = _mapper.Map<FeedbackResponse>(feedback);
            data.PostId = post.Id;

            return ResponseUtil.GetObject(data, "Feedback retrieved successfully", HttpStatusCode.OK, 1);
        }

        public async Task<ResponseDTO> GetFeedbackByUserId(int id, int page, int limit)
        {
            Feedback? feedback = (await _feedbackRepository.Find(c => c.UserId == id)).SingleOrDefault();

            if (feedback == null)
            {
                return ResponseUtil.Error("Request fails", "Feedback not found", HttpStatusCode.BadRequest);
            }

            return await GetFeedbackToResponse(feedback);
        }
    }

}
