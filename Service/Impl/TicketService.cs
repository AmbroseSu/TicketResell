using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICategoryRepository _ticketCategoryRepository;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepository ticketRepository, ICategoryRepository ticketCategoryRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> CreateTicketAsync(TicketDTO ticket)
        {
            //tim ticket category 
            IEnumerable<Category?> category = await _ticketCategoryRepository.Find(c => c.Id == ticket.CategoryId && c.IsDeleted);

            if (category == null)
            {
                return ResponseUtil.Error("Request fails", "Category not found !", HttpStatusCode.BadRequest);
            }

            Ticket savedTicket = _mapper.Map<Ticket>(ticket);

            await _ticketRepository.SaveAsync(savedTicket);
            TicketDTO result = _mapper.Map<TicketDTO>(savedTicket);
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

        public async Task<ResponseDTO> UpdateTicketAsync(TicketDTO ticket)
        {
            IEnumerable<Ticket?> result = await IsTicketValid(ticket.Id);
            if (result == null)
            {
                return ResponseUtil.Error("Request fails", "Ticket not found !", HttpStatusCode.BadRequest);
            }

            Ticket newTicket = _mapper.Map<Ticket>(ticket);
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
