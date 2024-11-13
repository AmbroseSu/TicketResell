
using AutoMapper;
using BusinessObject;
using System.Net;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;
using BusinessObject.Enums;
using DataAccess.DTO;
using System.Transactions;
using BusinessObject.enums;
using Microsoft.VisualBasic;
using Repository.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch.Internal;
using System.Collections.Generic;


namespace Service.Impl
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRespository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IImageTicketRepository _imageTicketRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IFeedbackService _feedbackService;

        public PostService(IPostRepository postRespository, ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper, ICategoryRepository ticketCategoryRepository, IImageTicketRepository imageTicketRepository, IFeedbackRepository feedbackRepository, IFeedbackService feedbackService)
        {
            _postRespository = postRespository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _ticketCategoryRepository = ticketCategoryRepository;
            _imageTicketRepository = imageTicketRepository;
            _feedbackRepository = feedbackRepository;
            _feedbackService = feedbackService;
        }

        public async Task<ResponseDTO> CreatePost(NewPostRequest post)
        {
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == post.ticketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            if (ticket.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Ticket is deleted !", HttpStatusCode.BadRequest);
            }

            if (ticket.Status != TicketStatus.ACTIVE)
            {
                return ResponseUtil.Error("Request fails", "Ticket is not active !", HttpStatusCode.BadRequest);
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

            IEnumerable<Post?> isPostExist = (await _postRespository.Find(p => p.TicketId == post.ticketId));

            foreach (Post item in isPostExist)
            {
                if (item != null)
                {
                    if (item.Status == (PostStatus.ACTIVE))
                    {
                        return ResponseUtil.Error("Request fails", "There is a post active with this ticket!", HttpStatusCode.BadRequest);
                    }
                    else if (item.Status == (PostStatus.PENDING))
                    {
                        return ResponseUtil.Error("Request fails", "There is a post is waiting for verify with this ticket!, please wait for response or delete your renew your request", HttpStatusCode.BadRequest);
                    }
                }
            }

            Post newPost = _mapper.Map<Post>(post);
            newPost.Status = PostStatus.PENDING;
            //newPost.CreatedDate = DateTime.Now.ToUniversalTime();
            await _postRespository.SaveAsync(newPost);

            PostDTO postDTO = _mapper.Map<PostDTO>(newPost);

            return ResponseUtil.GetObject(postDTO, "Post created successfully", HttpStatusCode.OK, 0);
        }


        public async Task<ResponseDTO> DeletePost(int postId)
        {
            Post? result = (await _postRespository.Find(c => c.Id == postId)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            if (result.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Post is deleted !", HttpStatusCode.BadRequest);
            }

            result.IsDeleted = true;
            result.Status = PostStatus.CLOSED;
            await _postRespository.UpdateAsync(result);
            return ResponseUtil.GetObject(result, "Post delete successfully", HttpStatusCode.OK, 1);
        }

        public async Task<ResponseDTO> GetAllPosts(int page, int limit, PostStatus? status, String? searchTerm)
        {
            IEnumerable<Post?> result = new List<Post?>();
            if (status == null)
            {
                result = await _postRespository.Find(p => p.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
            }
            else
            {
                result = await _postRespository.Find(p => p.Title.ToLower().Contains(searchTerm.Trim().ToLower())
                && p.Status == status
                );

            }

            return await GetAllPostInfo(result.ToList(), page, limit);

        }

        public async Task<ResponseDTO> GetPostByPostId(int id)
        {
            Post? result = (await _postRespository.Find(c => c.Id == id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            return await GetPostInfo(result);
            //return ResponseUtil.GetObject(result, "Post retrieved successfully", HttpStatusCode.OK, 0);
        }

        public async Task<ResponseDTO> UpdateStatus(int postId, PostStatus status)
        {
            Post? post = (await _postRespository.Find(p => p.Id == postId)).SingleOrDefault();

            if (post == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            if (post.IsDeleted)
            {
                return ResponseUtil.Error("Request fails", "Post is deleted !", HttpStatusCode.BadRequest);
            }

            if (!StatusExtensions.IsValidEnum(status))
            {
                return ResponseUtil.Error("Request fails", "Invalid status !", HttpStatusCode.BadRequest);
            }

            if (post.Status == PostStatus.CLOSED)
            {
                return ResponseUtil.Error("Request fails", "Post status = CLOSED can not be update !", HttpStatusCode.BadRequest);
            }

            if (post.Status == PostStatus.ACTIVE)
            {
                if (status == PostStatus.PENDING || status == PostStatus.ACTIVE)
                {
                    return ResponseUtil.Error("Request fails", "Post status = ACTIVE can only be update to CLOSED !", HttpStatusCode.BadRequest);
                }
            }

            if (post.Status == PostStatus.PENDING)
            {
                if (status == PostStatus.PENDING)
                {
                    return ResponseUtil.Error("Request fails", "Post status currently is already PENDING !", HttpStatusCode.BadRequest);
                }
            }

            post.Status = status;
            await _postRespository.UpdateAsync(post);
            return ResponseUtil.GetObject(post, "Post status updated successfully", HttpStatusCode.OK, 1);

        }

        public async Task<ResponseDTO> GetPostByTicketId(int id, int page, int limit)
        {
            IEnumerable<Post?> result = await _postRespository.Find(c => c.TicketId == id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            return await GetAllPostInfo(result.ToList(), page, limit);

        }


        private async Task<ResponseDTO> GetAllPostInfo(List<Post?> result, int page, int limit)
        {
            List<PostResponse?> responseData = new List<PostResponse?>();

            //TicketResponse chỉ chứa 1 post vì chỉ lấy post Active hoặc ko lấy post nào
            //PostResponse sẽ lấy tất cả các post dựa theo kết quả trả về từ repo. Ảnh hưởng bởi search, status
            //Khác với TicketResponse chỉ có 1 ticket - 1 post. PostResponse sẽ có nhiều post và chỉ 1 ticket
            //Get all post sẽ có unique là ticket id vì 1 ticket có nhiều post
            //Kiểu map có key là ticket id và value là post id

            Dictionary<Ticket, List<Post>> TicketPostMap = new Dictionary<Ticket, List<Post>>();

            //Vòng lặp này dùng để set Ticket và Post vào map
            foreach (Post post in result)
            {
                Ticket? ticket = (await _ticketRepository.Find(t => t.Id == post.TicketId)).SingleOrDefault();

                if (ticket == null)
                {
                    return ResponseUtil.Error("Request fails", "Ticket not found in post !", HttpStatusCode.BadRequest);
                }

                //Nếu đã có key ticket trong map thì add post vào list
                if (TicketPostMap.ContainsKey(ticket))
                {
                    TicketPostMap[ticket].Add(post);
                }
                else //Nếu chưa có key thì tạo mới key => thêm value
                {
                    TicketPostMap.Add(ticket, new List<Post>() { post });
                }

            }

            foreach (KeyValuePair<Ticket, List<Post>> tpm in TicketPostMap)
            {

                Category? category = (await _ticketCategoryRepository.Find(c => c.Id == tpm.Key.CategoryId)).SingleOrDefault();

                if (category == null)
                {
                    return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
                }

                User? user = await _userRepository.FindUserByIdAsync(tpm.Key.UserId);
                if (user == null)
                {
                    return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
                }

                PostResponse postResponse = new PostResponse();

                DateTime localExpiredTime = tpm.Key.ExpirationDate;
                //DateTime localCreatedDateTime = tpm.Key.CreatedDate;
                localExpiredTime = localExpiredTime.ToLocalTime();
                //localCreatedDateTime = localCreatedDateTime.ToLocalTime();
                postResponse.ExpirationDate = localExpiredTime;
                //item.CreatedDate = localCreatedDateTime;

                List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == tpm.Key.Id)).ToList();
                List<FeedbackResponse> feedbacks = (List<FeedbackResponse>)_feedbackService.GetFeedBacksByTicketId(tpm.Key.Id, 1, 100).Result.Content;
                List<FeedbackTicketElement> elements = _mapper.Map<List<FeedbackTicketElement>>(feedbacks);

                _mapper.Map(tpm.Key, postResponse);
                _mapper.Map(category, postResponse);
                _mapper.Map(user, postResponse);
                postResponse.feedbackDTOs = elements;

                if (imageTickets.Count != 0)
                {
                    List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
                    postResponse.imageTicketDTOs = imgList;
                }
                else
                {
                    postResponse.imageTicketDTOs = null;
                }

                List<PostElement> postElements = _mapper.Map<List<PostElement>>(tpm.Value);

                foreach (PostElement pe in postElements)
                {
                    pe.CreatedDate = pe.CreatedDate.ToLocalTime();
                }
                postResponse.PostElements = postElements;
                responseData.Add(postResponse);
            }

            List<PostResponse?> data = responseData.Skip((page - 1) * limit).Take(limit).ToList();
            return ResponseUtil.GetCollection(data, "All posts retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        }

        private async Task<ResponseDTO> GetPostInfo(Post? result)
        {
            PostResponse? responseData = new();
            List<PostElement> postElements = new List<PostElement>();
            PostElement postElement = _mapper.Map<PostElement>(result);
            postElements.Add(postElement);
            responseData.PostElements = postElements;

            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == result.TicketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            Category? category = (await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId)).SingleOrDefault();

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            User? user = await _userRepository.FindUserByIdAsync(ticket.UserId);
            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }
            PostResponse postResponse = new PostResponse();

            DateTime localExpiredTime = ticket.ExpirationDate;
            DateTime localCreatedDateTime = result.CreatedDate;
            localExpiredTime = localExpiredTime.ToLocalTime();
            localCreatedDateTime = localCreatedDateTime.ToLocalTime();
            responseData.ExpirationDate = localExpiredTime;
            result.CreatedDate = localCreatedDateTime;

            List<ImageTicket?> imageTickets = (await _imageTicketRepository.Find(i => i.TicketId == ticket.Id)).ToList();
            List<FeedbackResponse> feedbacks = (List<FeedbackResponse>)_feedbackService.GetFeedBacksByTicketId(ticket.Id, 1, 100).Result.Content;
            List<FeedbackTicketElement> elements = _mapper.Map<List<FeedbackTicketElement>>(feedbacks);

            _mapper.Map(ticket, responseData);
            _mapper.Map(category, responseData);
            _mapper.Map(user, responseData);
            responseData.feedbackDTOs = elements;

            if (imageTickets.Count != 0)
            {
                List<ImageTicketDTO> imgList = _mapper.Map<List<ImageTicketDTO>>(imageTickets);
                responseData.imageTicketDTOs = imgList;
            }
            else
            {
                responseData.imageTicketDTOs = null;
            }

            return ResponseUtil.GetObject(responseData, "Post retrieved sucessfully", HttpStatusCode.OK, 1);
        }

        public async Task<ResponseDTO> GetPostByUserId(int id, PostStatus? status, int page, int limit)
        {
            User? user = await _userRepository.FindUserByIdAsync(id);

            if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }

            IEnumerable<Ticket?> tickets = await _ticketRepository.Find(t => t.UserId == id);

            if (tickets == null)
            {
                return ResponseUtil.GetCollection(null, "No post found !", HttpStatusCode.OK, 0, page, limit, 0);
            }

            List<Post?> posts = await GetAllPostByListTicket(tickets.ToList(), status);

            return await GetAllPostInfo(posts, page, limit);
        }

        public async Task<ResponseDTO> GetPostByCategoryId(int id, PostStatus? status, int page, int limit)
        {
            Category? category = (await _ticketCategoryRepository.Find(c => c.Id == id)).SingleOrDefault();

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            IEnumerable<Ticket?> tickets = await _ticketRepository.Find(t => t.CategoryId == id);

            if (tickets == null)
            {
                return ResponseUtil.GetCollection(null, "No post found !", HttpStatusCode.OK, 0, page, limit, 0);
            }
            List<Post?> posts = await GetAllPostByListTicket(tickets.ToList(), status);
            return await GetAllPostInfo(posts, page, limit);
        }

        private async Task<List<Post?>> GetAllPostByListTicket(List<Ticket> tickets, PostStatus? status)
        {
            List<Post?> posts = new List<Post?>();

            foreach (Ticket ticket in tickets)
            {
                if (status != null)
                {
                    IEnumerable<Post?> post = await _postRespository.Find(p => p.TicketId == ticket.Id && p.Status == status);
                    posts.AddRange(post);
                }
                else
                {
                    IEnumerable<Post?> post = await _postRespository.Find(p => p.TicketId == ticket.Id);
                    posts.AddRange(post);
                }
            }

            return posts;
        }
    }
}


