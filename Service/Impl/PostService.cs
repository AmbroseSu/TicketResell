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
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRespository, ITicketRepository ticketRepository, IMapper mapper)
        {
            _postRespository = postRespository;
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> CreatePost(NewPostRequest post, int TicketId, int userId)
        {
            Ticket? ticket = (await _ticketRepository.Find(t => t.Id == TicketId)).SingleOrDefault();

            if (ticket == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }
            post.TicketId = TicketId;
            post.UserId = userId;
            Post savedPost = _mapper.Map<Post>(post);
            //savedPost.Status = PostStatus.PENDING;
            await _postRespository.SaveAsync(savedPost);

            return ResponseUtil.GetObject(null, "Post created successfully", HttpStatusCode.OK, null);
        }

        public Task<ResponseDTO> DeletePost(int TicketId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> EditPost(int TicketId, string description)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> getAllPosts(int page, int limit)
        {
            IEnumerable<Post?> result = await _postRespository.GetAllAsync();
            IEnumerable<Post?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All posts retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());
        }

        public async Task<ResponseDTO> getCurrentPosts(int page, int limit)
        {
            IEnumerable<Post?> result = await _postRespository.Find(p => p.IsDeleted == false );
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
