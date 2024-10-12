using AutoMapper;
using BusinessObject;
using System.Net;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;


namespace Service.Impl
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRespository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRespository, ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper)
        {
            _postRespository = postRespository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> CreatePost(NewPostRequest post, int TicketId, int userId)
        {
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == TicketId)).SingleOrDefault();
            User? user = await _userRepository.FindUserByIdAsync(userId);

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }
            else if (user == null)
            {
                return ResponseUtil.Error("Request fails", "User not found !", HttpStatusCode.BadRequest);
            }
            Post savedPost = _mapper.Map<Post>(post);
            savedPost.TicketId = TicketId;
            savedPost.UserId = userId;
            //savedPost.Status = PostStatus.PENDING;
            await _postRespository.SaveAsync(savedPost);

            return ResponseUtil.GetObject(savedPost, "Post created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> DeletePost(int TicketId)
        {
            Post? result = (await _postRespository.Find(c => c.IsDeleted == false && c.Id == TicketId)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }
            //result.Status = Post.Status;
            await _postRespository.DeleteAsync(TicketId);
            return ResponseUtil.GetObject(result, "Post created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> EditPost(int TicketId, string description)
        {
            Post? result = (await _postRespository.Find(c => c.IsDeleted == false && c.Id == TicketId)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }
            result.Description = description;
            await _postRespository.UpdateAsync(result);
            return ResponseUtil.GetObject(result, "Post created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> getAllPosts(int page, int limit)
        {
            IEnumerable<Post?> result = await _postRespository.GetAllAsync();
            IEnumerable<Post?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All posts retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());
        }

        public async Task<ResponseDTO> getCurrentPosts(int page, int limit)
        {
            IEnumerable<Post?> result = await _postRespository.Find(p => p.IsDeleted == false && p.Status.Equals("OPEN") );
            IEnumerable<Post?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All available posts retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());
        }

        public async Task<ResponseDTO> GetPost(int id)
        {
            Post? result = (await _postRespository.Find(c => c.IsDeleted == false && c.Id == id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
            }

            return ResponseUtil.GetObject(result, "Post retrieved successfully", HttpStatusCode.OK, null);
        }
    }
}
