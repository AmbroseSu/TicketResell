using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRespository;
        private readonly ITicketRepository _ticketRepository;

        public PostService(IPostRepository postRepository, ITicketRepository ticketRepository)
        {
            _postRespository = postRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<ResponseDTO> CreatePost(PostDTO post, int ticketId)
        {
            IEnumerable<Ticket?> savedTicket = await _ticketRepository.Find(ticket => ticket.Id == ticketId);


            return null; 
        }

        public Task<ResponseDTO> EditPost(PostDTO post)
        {
            throw new NotImplementedException();
        }
    }
}
