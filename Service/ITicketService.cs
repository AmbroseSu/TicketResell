using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{
    public interface ITicketService
    {
        Task<ResponseDTO> CreateTicket(NewTicket post);
        //Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket);
        Task<ResponseDTO> DeleteTicketAsync(int id);
        Task<ResponseDTO> GetTicketAsync(int id);
        Task<ResponseDTO> GetAllTicket(int page, int limit, TicketStatus? status, String? searchTerm);
        Task<ResponseDTO> UpdateStatus(int id, TicketStatus status);
        Task<ResponseDTO> GetTicketByCategoryId(int id, int page, int limit);
        Task<ResponseDTO> GetTicketByUserId(int id, TicketStatus? status, int page, int limit);
        Task<ResponseDTO> UpdateTicketImg(List<string> imgList, int ticketId);
        Task<ResponseDTO> getTicketInfoResponse(Ticket result);
        Task<ResponseDTO> getListTicketInforResponse(List<Ticket?> result, int page, int limit);
    }
}
