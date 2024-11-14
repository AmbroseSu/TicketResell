using AutoMapper;
using BusinessObject;
using BusinessObject.enums;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Repository.Impl;
using Service.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IImageFeedbackRepository _imageFeedbackRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;

        public FeedbackService(ITicketRepository ticketRepository, IFeedbackRepository feedbackRepository, IPostRepository postRepository, IUserRepository userRepository, IOrderRepository orderRepository, IMapper mapper, IImageFeedbackRepository imageFeedbackRepository, IOrderStatusRepository orderStatusRepository)
        {
            _ticketRepository = ticketRepository;
            _feedbackRepository = feedbackRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _imageFeedbackRepository = imageFeedbackRepository;
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<ResponseDTO> AddFeedback(NewFeedback feedback)
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

            //kiểm tra xem user có đặt hàng ko
            IEnumerable<Order?> order = await _orderRepository.Find(c => c.TicketId == ticket.Id);

            bool isOrder = false;
            if (order == null)
            {
                return ResponseUtil.Error("Request fails", "Order not found to be feedback", HttpStatusCode.BadRequest);
            }

            foreach (Order? item in order.ToList())
            {
                IEnumerable<OrderStatus?> orderStatus = await _orderStatusRepository.Find(c => c.OrderId == item.Id);

                if (orderStatus == null)
                {
                    return ResponseUtil.Error("Request fails", "Order status not found to be feedback", HttpStatusCode.BadRequest);
                }

                foreach (OrderStatus status in orderStatus.ToList())
                {
                    if (status.Name.Equals("Done"))
                    {
                        isOrder = true;
                        break;
                    }

                }

            }

            if (!isOrder)
            {
                return ResponseUtil.Error("Request fails", "Order not found to be feedback", HttpStatusCode.BadRequest);
            }

            //post có tồn tại ko
            Post? post = (await _postRepository.Find(c => c.TicketId == ticket.Id)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found", HttpStatusCode.BadRequest);
            }
            else
            {
                if (post.Status != PostStatus.ACTIVE)
                {
                    return ResponseUtil.Error("Request fails", "Post status is not active to be feedback", HttpStatusCode.BadRequest);
                }
            }
            //Kiểm tra xem user này đã feedback chưa
            Feedback? feedbackExist = (await _feedbackRepository.Find(c => c.UserId == feedback.UserId && c.TicketId == feedback.TicketId)).SingleOrDefault();

            if (feedbackExist != null)
            {
                return ResponseUtil.Error("Request fails", "User has already feedback", HttpStatusCode.BadRequest);
            }

            Feedback result = _mapper.Map<Feedback>(feedback);
            //Tạo prefix Time
            String prefixDateTimeNow = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

            result.Context = "[" + prefixDateTimeNow + "] " + result.Context;
            await _feedbackRepository.SaveAsync(result);
            return ResponseUtil.GetObject(result, "Feedback added successfully", HttpStatusCode.Created, 0);
        }

        public async Task<ResponseDTO> DeleteFeedback(int id)
        {
            Feedback? feedback = (await _feedbackRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (feedback == null)
            {
                return ResponseUtil.Error("Request fails", "Feedback not found", HttpStatusCode.NotFound);
            }

            if (feedback.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Feedback is already deleted", HttpStatusCode.BadRequest);
            }

            feedback.IsDeleted = true;
            await _feedbackRepository.UpdateAsync(feedback);

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

                List<ImageFeedback?> imgs = (await _imageFeedbackRepository.Find(i => i.FeedbackId == feedback.Id)).ToList();

                if (imgs.Count != 0)
                {
                    List<ImageFeedbackDTO> imgList = _mapper.Map<List<ImageFeedbackDTO>>(imgs);
                    feedback.imgs = imgList;
                }
                else
                {
                    feedback.imgs = null;
                }
                feedback.PostId = post.Id;
                feedback.FullName = user.Fullname;
                //Trim là bỏ phần trước [ 
                //Split là tách chuỗi, ở đây tách xong lấy phần tử 0
                string format = "yyyy-MM-dd HH:mm:ss";
                string[] context = item.Context.Split(']');
                string prefix = context[0].TrimStart('[');
                feedback.Context = context[1].TrimStart(' ');
                DateTime time = DateTime.ParseExact(prefix, format,
                    CultureInfo.InvariantCulture);

                feedback.CreatedDate = time;
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

            List<ImageFeedback?> imgs = (await _imageFeedbackRepository.Find(i => i.FeedbackId == feedback.Id)).ToList();
            FeedbackResponse data = _mapper.Map<FeedbackResponse>(feedback);

            if (imgs.Count != 0)
            {
                List<ImageFeedbackDTO> imgList = _mapper.Map<List<ImageFeedbackDTO>>(imgs);
                data.imgs = imgList;
            }
            else
            {
                data.imgs = null;
            }

            data.PostId = post.Id;
            data.FullName = user.Fullname;
            //Trim là bỏ phần trước [ 
            //Split là tách chuỗi, ở đây tách xong lấy phần tử 0
            string format = "yyyy-MM-dd HH:mm:ss";
            string[] context = feedback.Context.Split(']');
            string prefix = context[0].TrimStart('[');
            data.Context = context[1].TrimStart(' ');
            DateTime time = DateTime.ParseExact(prefix, format,
                CultureInfo.InvariantCulture);

            data.CreatedDate = time;
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

        public async Task<ResponseDTO> UploadImg(List<String> imgs, int feedbackId)
        {

            Feedback? feedback = (await _feedbackRepository.Find(t => t.Id == feedbackId)).SingleOrDefault();

            if (feedback == null)
            {
                return ResponseUtil.Error("Request fails", "Feedback not found !", HttpStatusCode.BadRequest);
            }

            if (feedback.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Feedback is deleted !", HttpStatusCode.BadRequest);
            }

            List<ImageFeedback?> images = (await _imageFeedbackRepository.Find(i => i.FeedbackId == feedbackId)).ToList();

            if (images.Count != 0)
            {
                return ResponseUtil.Error("Request fails", "Image already exists !", HttpStatusCode.BadRequest);
            }

            foreach (string imgUrl in imgs)
            {
                ImageFeedback image = new ImageFeedback()
                {
                    ImageUrl = imgUrl,
                    FeedbackId = feedbackId

                };
                await _imageFeedbackRepository.SaveAsync(image);
            }

            return ResponseUtil.GetObject("Request accepted", "Image updated successfully", HttpStatusCode.Accepted, 0);

        }

        public ResponseDTO GetUserReputation(int userId)
        {
            float repu = 0;
            int TicketCount = 0;
            float totalTicketRating = 0;
            User? user = _userRepository.FindUserByIdAsync(userId).Result;

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found", HttpStatusCode.BadRequest);
            }

            List<Ticket?> tickets = _ticketRepository.Find(t => t.UserId == userId).Result.ToList();

            if (tickets != null)
            {
                TicketCount = tickets.Count;
                foreach (Ticket ticket in tickets)
                {
                    List<Feedback?> feedbacks = _feedbackRepository.Find(f => f.TicketId == ticket.Id).Result.ToList();

                    if (feedbacks != null)
                    {
                        TicketCount--;
                        int totalRating = 0;
                        foreach (Feedback feedback in feedbacks)
                        {
                            totalRating += feedback.Rating;
                        }

                        float ticketRating = totalRating / feedbacks.Count;
                        totalTicketRating += ticketRating;
                    }

                }
                repu = totalTicketRating / TicketCount;
            }

            return ResponseUtil.GetObject(repu, "Return user points successfully", HttpStatusCode.OK, 0);
        }
    }

}
