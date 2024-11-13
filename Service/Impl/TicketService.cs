
using AutoMapper;
using BusinessObject;
using BusinessObject.enums;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Impl;
using Service.Response;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;


namespace Service.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageTicketRepository _imageTicketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IFeedbackService _feedbackService;

        public TicketService(ITicketRepository ticketRepository, ICategoryRepository ticketCategoryRepository, IMapper mapper, IImageTicketRepository imageTicketRepository, IUserRepository userRepository, IPostRepository postRepository, IFeedbackService feedbackService)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _mapper = mapper;
            _imageTicketRepository = imageTicketRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _feedbackService = feedbackService;
        }

        public async Task<ResponseDTO> CreateTicket(NewTicket ticket)
        {
            Ticket? result = (await _ticketRepository.Find(t => t.Name.ToLower().Trim().Equals(ticket.Name.ToLower().Trim()))).SingleOrDefault();

            if (result != null)
            {
                return ResponseUtil.Error("Request fails", "Ticket already exists !", HttpStatusCode.BadRequest);
            }

            User? user = await _userRepository.FindUserByIdAsync(ticket.UserId);

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found", HttpStatusCode.BadRequest);
            }

            if (user.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "User is deleted", HttpStatusCode.BadRequest);
            }

            if (!user.IsEnabled)
            {
                return ResponseUtil.Error("Request fails", "User is disabled", HttpStatusCode.BadRequest);
            }

            //Create ticket
            //tim ticket category
            Category? category = (await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId && c.IsDeleted == false)).SingleOrDefault();

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            Ticket reqTicket = _mapper.Map<Ticket>(ticket);
            reqTicket.Status = TicketStatus.PENDING;

            string format = "dd/MM/yyyy HH:mm";

            // Kiểm tra và chuyển đổi ExpirationDate
            if (!DateTime.TryParseExact(ticket.ExpirationDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiredDate))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date format!", HttpStatusCode.BadRequest);
            }

            // Thời gian hiện tại theo LocalTime
            DateTime currentTime = DateTime.Now;

            // Kiểm tra điều kiện ngày hết hạn
            if (expiredDate < currentTime.AddDays(1) || expiredDate > currentTime.AddYears(1))
            {
                return ResponseUtil.Error("Request fails", "Invalid expiration date!", HttpStatusCode.BadRequest);
            }

            // Chuyển expiredDate sang UTC và gán vào reqTicket
            reqTicket.ExpirationDate = expiredDate.ToUniversalTime();

            // Lưu ticket vào repository
            await _ticketRepository.SaveAsync(reqTicket);


            //Commit transaction

            return ResponseUtil.GetObject(reqTicket, "Ticket created successfully", HttpStatusCode.OK, 0);
        }

        public async Task<ResponseDTO> DeleteTicketAsync(int id)
        {
            Ticket? result = await IsTicketValid(id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            if (result.IsDeleted == true)
            {
                return ResponseUtil.Error("Request fails", "Ticket already deleted !", HttpStatusCode.BadRequest);
            }

            result.IsDeleted = true;
            result.Status = TicketStatus.CLOSED;
            await _ticketRepository.DeleteAsync(id);
            return ResponseUtil.GetObject("Request accepted", "Ticket Deleted successfully", HttpStatusCode.Accepted, 0);
        }

        public async Task<ResponseDTO> GetTicketAsync(int id)
        {
            Ticket? result = (await _ticketRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.GetObject(result, "Ticket retrieved successfully", HttpStatusCode.OK, 0);
            }

            return await getTicketInfoResponse(result);
        }

        public async Task<ResponseDTO> GetAllTicket(int page, int limit, TicketStatus? status, String? searchTerm)
        {
            IEnumerable<Ticket?> result = new List<Ticket?>();
            if (status == null)
            {
                result = await _ticketRepository.Find(t => t.Name.ToLower().Contains(searchTerm.Trim()));
            }
            else
            {
                result = await _ticketRepository.Find(t => t.Name.ToLower().Contains(searchTerm.Trim()) &&
               t.Status == status);
            }

            if (result == null)
            {
                return ResponseUtil.GetObject(result, "Ticket retrieved successfully", HttpStatusCode.OK, 0);
            }

            return await getListTicketInforResponse(result.ToList(), page, limit);
        }

        /// <summary>
        /// Update ticket status cho manager, chỉ có thể update từ pending -> active, closed hoặc active -> closed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateStatus(int id, TicketStatus status)
        {
            if (!StatusExtensions.IsValidEnum(status))
            {
                return ResponseUtil.Error("Request fails", "Invalid status !", HttpStatusCode.BadRequest);
            }

            Ticket? result = (await IsTicketValid(id));

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            if (result.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Ticket already deleted !", HttpStatusCode.BadRequest);
            }

            if (result.Status == TicketStatus.CLOSED)
            {
                return ResponseUtil.Error("Request fails", "Ticket status = CLOSED can not be update !", HttpStatusCode.BadRequest);
            }

            if (result.Status == TicketStatus.ACTIVE)
            {
                if (status == TicketStatus.PENDING || status == TicketStatus.ACTIVE)
                {
                    return ResponseUtil.Error("Request fails", "Ticket status = ACTIVE can only be update to CLOSED !", HttpStatusCode.BadRequest);
                }
            }

            if (result.Status == TicketStatus.PENDING)
            {
                if (status == TicketStatus.PENDING)
                {
                    return ResponseUtil.Error("Request fails", "Ticket status currently is already PENDING !", HttpStatusCode.BadRequest);
                }
            }

            result.Status = status;
            await _ticketRepository.UpdateAsync(result);

            return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, 0);
        }

        //public async Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket)
        //{
        //    Ticket? result = (await IsTicketValid(ticket.Id));
        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
        //    }

        //    result.Venue = ticket.Venue;
        //    result.Quantity = ticket.Quantity;
        //    result.Status = ticket.Status;

        //    Ticket newTicket = _mapper.Map<Ticket>(result);
        //    await _ticketRepository.UpdateAsync(newTicket);
        //    return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, 0);
        //}

        private async Task<Ticket> IsTicketValid(int ticketId)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.Find(t => t.Id == ticketId);

            if (result == null)
            {
                return null;
            }

            return result.SingleOrDefault();
        }

        private TicketResponse getTicketInfo(Ticket result, Category cat, User user, Post? post, List<FeedbackResponse>? feedbacks)
        {
            TicketResponse ticketResponse = new();

            return ticketResponse;
        }

        public async Task<ResponseDTO> GetTicketByCategoryId(int id, int page, int limit)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.Find(c => c.CategoryId == id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            return await getListTicketInforResponse(result.ToList(), page, limit);

        }

        private async Task<ResponseDTO> getTicketInfoResponse(Ticket result)
        {
            Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == result.CategoryId)).SingleOrDefault();

            if (cat == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            User? user = await _userRepository.FindUserByIdAsync(result.UserId);

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }

            Post? post = (await _postRepository.Find(p => p.TicketId == result.Id && p.Status == PostStatus.ACTIVE)).SingleOrDefault();

            List<FeedbackResponse> feedbacks = (List<FeedbackResponse>)_feedbackService.GetFeedBacksByTicketId(result.Id, 1, 100).Result.Content;
            TicketResponse ticket = _mapper.Map<TicketResponse>(result);
            _mapper.Map(cat, ticket);
            _mapper.Map(user, ticket);

            if (post != null)
            {
                _mapper.Map(post, ticket);
            }

            List<FeedbackTicketElement> elements = _mapper.Map<List<FeedbackTicketElement>>(feedbacks);
            ticket.feedbackDTOs = elements;
            List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == result.Id)).ToList();

            if (imageTickets.Count != 0)
            {
                List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
                ticket.imageTicketDTOs = imgList;
            }
            else
            {
                ticket.imageTicketDTOs = null;
            }
            DateTime localExpiredTime = ticket.ExpirationDate;
            DateTime localCreatedDateTime = ticket.CreatedDate;
            localExpiredTime = localExpiredTime.ToLocalTime();
            localCreatedDateTime = localCreatedDateTime.ToLocalTime();
            ticket.ExpirationDate = localExpiredTime;
            ticket.CreatedDate = localCreatedDateTime;
            return ResponseUtil.GetObject(ticket, "Ticket retrieved successfully", HttpStatusCode.OK, 1);
        }

        private async Task<ResponseDTO> getListTicketInforResponse(List<Ticket?> result, int page, int limit)
        {
            List<TicketResponse?> responseData = new List<TicketResponse?>();

            //Duyệt qua list ticket lấy ticket info tương ứng

            foreach (Ticket ticket in result)
            {

                Category? cat = (await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId)).SingleOrDefault();

                if (cat == null)
                {
                    return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
                }

                User? user = (await _userRepository.FindUserByIdAsync(ticket.UserId));

                if (user == null)
                {
                    return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
                }

                Post? post = (await _postRepository.Find(p => p.TicketId == ticket.Id && p.Status == PostStatus.ACTIVE)).SingleOrDefault();

                DateTime localExpiredTime = ticket.ExpirationDate;
                localExpiredTime = localExpiredTime.ToLocalTime();
                ticket.ExpirationDate = localExpiredTime;

                if (post != null)
                {
                    DateTime localCreatedDateTime = post.CreatedDate;
                    localCreatedDateTime = localCreatedDateTime.ToLocalTime();
                    post.CreatedDate = localCreatedDateTime;

                }

                List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticket.Id)).ToList();
                List<FeedbackResponse> feedbacks = (List<FeedbackResponse>)_feedbackService.GetFeedBacksByTicketId(ticket.Id, 1, 100).Result.Content;
                TicketResponse ticketResponse = _mapper.Map<TicketResponse>(ticket);
                _mapper.Map(cat, ticketResponse);
                _mapper.Map(user, ticketResponse);

                if (post != null)
                {
                    _mapper.Map(post, ticketResponse);
                }

                List<FeedbackTicketElement> elements = _mapper.Map<List<FeedbackTicketElement>>(feedbacks);
                ticketResponse.feedbackDTOs = elements;

                if (imageTickets.Count != 0)
                {
                    List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
                    ticketResponse.imageTicketDTOs = imgList;
                }
                else
                {
                    ticketResponse.imageTicketDTOs = null;
                }

                responseData.Add(ticketResponse);
            }

            List<TicketResponse?> data = responseData.Skip((page - 1) * limit).Take(limit).ToList();
            return ResponseUtil.GetCollection(data, "All tickets retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        }

        public async Task<ResponseDTO> GetTicketByUserId(int id, int page, int limit)
        {
            User? user = await _userRepository.FindUserByIdAsync(id);

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }

            IEnumerable<Ticket?> tickets = await _ticketRepository.Find(p => p.UserId == user.Id);

            if (tickets.Count() == 0)
            {
                return ResponseUtil.GetObject("Request accepted", "No ticket found !", HttpStatusCode.Accepted, 0);
            }

            List<Ticket?> ticketList = new List<Ticket?>();

            foreach (var item in tickets)
            {
                Ticket? result = (await _ticketRepository.Find(t => t.Id == item.Id)).SingleOrDefault();

                if (result == null)
                {
                    return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
                }

                ticketList.Add(result);
            }

            return await getListTicketInforResponse(ticketList, page, limit);

        }

        public async Task<ResponseDTO> UpdateTicketImg(List<string> imgList, int ticketId)
        {
            Ticket ticket = (await _ticketRepository.Find(t => t.Id == ticketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticketId)).ToList();

            if (imageTickets.Count != 0)
            {
                return ResponseUtil.Error("Request fails", "Image already exists !", HttpStatusCode.BadRequest);
            }

            foreach (string imgUrl in imgList)
            {
                ImageTicket image = new ImageTicket()
                {
                    ImageUrl = imgUrl,
                    IsDeleted = false,
                    TicketId = ticketId
                };
                await _imageTicketRepository.SaveAsync(image);
            }

            return ResponseUtil.GetObject("Request accepted", "Image updated successfully", HttpStatusCode.Accepted, 0);
        }
    }
}

