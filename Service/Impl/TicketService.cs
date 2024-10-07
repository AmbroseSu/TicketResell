using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Repository;
using Repository.Impl;
using Service.Response;
using System.Net;


namespace Service.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageTicketRepository _imageTicketRepository;

        public TicketService(ITicketRepository ticketRepository, ICategoryRepository ticketCategoryRepository, IMapper mapper, IImageTicketRepository imageTicketRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _mapper = mapper;
            _imageTicketRepository = imageTicketRepository;
        }

        public async Task<ResponseDTO> CreateTicketAsync(NewTicketRequest ticket, string imgUrl)
        {
            //tim ticket category 
            IEnumerable<Category?> category = await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId && c.IsDeleted);

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            Ticket reqTicket = _mapper.Map<Ticket>(ticket);

            ImageTicket image = new ImageTicket()
            {
                ImageUrl = imgUrl,
                IsDeleted = false,
                TicketId = reqTicket.Id
                
            };

            reqTicket.Status = TicketStatus.PENDING;
            await _ticketRepository.SaveAsync(reqTicket);
            await _imageTicketRepository.SaveAsync(image);
            TicketDTO result = _mapper.Map<TicketDTO>(reqTicket);
            return ResponseUtil.GetObject(result, "Ticket created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> DeleteTicketAsync(int id)
        {
            IEnumerable<Ticket?> result = await IsTicketValid(id);

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            await _ticketRepository.DeleteAsync(id);

            return ResponseUtil.GetObject("Request accepted", "Ticket Deleted successfully", HttpStatusCode.Accepted, null);
        }

        public async Task<ResponseDTO> GetTicketAsync(int id)
        {
            Ticket? result = (await _ticketRepository.Find(c => c.IsDeleted == false && c.Id == id)).SingleOrDefault();

            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            return ResponseUtil.GetObject(result, "Ticket retrieved successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> GetTicketsAsync(int page, int limit)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.GetAllAsync();
            IEnumerable<Ticket?> data = result.Skip((page - 1) * limit).Take(limit);
            return ResponseUtil.GetCollection(data, "All tickets retrieved sucessfully", HttpStatusCode.OK, page, limit, result.Count());
        }

        public async Task<ResponseDTO> updateStatus(int id, string status)
        {
            Ticket? result = (await IsTicketValid(id)).SingleOrDefault();
            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            //result.Status = status;

            Ticket newTicket = _mapper.Map<Ticket>(result);
            await _ticketRepository.UpdateAsync(newTicket);

            return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, null);
        }

        public async Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket)
        {
            Ticket? result = (await IsTicketValid(ticket.Id)).SingleOrDefault();
            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            result.Quantity = ticket.Quantity;
            result.Status = ticket.Status;
            result.Status = ticket.Status;

            Ticket newTicket = _mapper.Map<Ticket>(result);
            await _ticketRepository.UpdateAsync(newTicket);
            return ResponseUtil.GetObject("Request accepted", "Ticket Updated successfully", HttpStatusCode.Accepted, null);
        }

        private async Task<IEnumerable<Ticket>> IsTicketValid(int ticketId)
        {
            IEnumerable<Ticket?> result = await _ticketRepository.Find(t => t.Id == ticketId && t.IsDeleted == false);

            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}
