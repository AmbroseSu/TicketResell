
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


namespace Service.Impl
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRespository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _ticketCategoryRepository;

        public PostService(IPostRepository postRespository, ITicketRepository ticketRepository, IUserRepository userRepository, IMapper mapper, ICategoryRepository ticketCategoryRepository)
        {
            _postRespository = postRespository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public async Task<ResponseDTO> CreatePost(NewPostRequest post)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Ticket? ticket = (await _ticketRepository.Find(t => t.Id == post.ticketId)).SingleOrDefault();

                if (ticket == null)
                {
                    return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
                }

                Post? isPostExist = (await _postRespository.Find(p => p.TicketId == post.ticketId && p.Status.Equals(PostStatus.ACTIVE ))).SingleOrDefault();

                if (isPostExist != null )
                {
                    return ResponseUtil.Error("Request fails", "There is a post active with this ticket!", HttpStatusCode.BadRequest);
                }

                Post newPost = _mapper.Map<Post>(post);

                    Ticket reqTicket = _mapper.Map<Ticket>(post);
                reqTicket.Status = TicketStatus.PENDING;
                await _ticketRepository.SaveAsync(reqTicket);

                Commit transaction

                Post savedPost = _mapper.Map<Post>(post);
                savedPost.Status = false;
                await _postRespository.SaveAsync(savedPost);


                PostDTO postDTO = _mapper.Map<PostDTO>(savedPost);

                scope.Complete();

                return ResponseUtil.GetObject(postDTO, "Post created successfully", HttpStatusCode.OK, 0);
            }
        }

        //public async Task<ResponseDTO> DeletePost(int TicketId)
        //{
        //    Post? result = (await _postRespository.Find(c => c.Id == TicketId && c.IsDeleted == false)).SingleOrDefault();

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
        //    }
        //    result.Status = Post.Status;
        //    await _postRespository.DeleteAsync(TicketId);
        //    return ResponseUtil.GetObject(result, "Post created successfully", HttpStatusCode.OK, 0);
        //}

        //public async Task<ResponseDTO> EditPost(int TicketId, string description)
        //{
        //    Post? result = (await _postRespository.Find(c => c.Id == TicketId)).SingleOrDefault();

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
        //    }
        //    result.Description = description;
        //    await _postRespository.UpdateAsync(result);
        //    return ResponseUtil.GetObject(result, "Post created successfully", HttpStatusCode.OK, 0);
        //}

        //public async Task<ResponseDTO> getAllPosts(int page, int limit)
        //{
        //    IEnumerable<Post?> result = await _postRespository.GetAllAsync();
        //    IEnumerable<Post?> data = result.Skip((page - 1) * limit).Take(limit);
        //    return ResponseUtil.GetCollection(data, "All posts retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        //}

        //public async Task<ResponseDTO> getCurrentPosts(int page, int limit)
        //{
        //    IEnumerable<Post?> result = await _postRespository.Find(p => p.Status.Equals("OPEN"));
        //    IEnumerable<Post?> data = result.Skip((page - 1) * limit).Take(limit);
        //    return ResponseUtil.GetCollection(data, "All available posts retrieved sucessfully", HttpStatusCode.OK, result.Count(), page, limit, result.Count());
        //}

        //public async Task<ResponseDTO> GetPost(int id)
        //{
        //    Post? result = (await _postRespository.Find(c => c.Id == id)).SingleOrDefault();

        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
        //    }

        //    return ResponseUtil.GetObject(result, "Post retrieved successfully", HttpStatusCode.OK, 0);
        //}

        //public async Task<ResponseDTO> PostVerify(int id)
        //{
        //    Post? result = (await _postRespository.Find(c => c.Id == id)).SingleOrDefault();
        //    if (result == null)
        //    {
        //        return ResponseUtil.Error("Request fails", "Post not found !", HttpStatusCode.BadRequest);
        //    }
        //    result.Status = true;
        //    await _postRespository.UpdateAsync(result);
        //    return ResponseUtil.GetObject(result, "Post verified successfully", HttpStatusCode.OK, 0);
        //}
    }
}

