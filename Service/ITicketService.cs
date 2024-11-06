using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITicketService
    {
        Task<ResponseDTO> CreateTicket(NewTicketRequest post);
        Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket);
        Task<ResponseDTO> DeleteTicketAsync(int id);
        Task<ResponseDTO> GetTicketAsync(int id);
        Task<ResponseDTO> GetTicketsAsync(int page, int limit);
        Task<ResponseDTO> UpdateStatus(int id, string status);
        Task<ResponseDTO> GetTicketByCategoryId(int id, int page, int limit);
        Task<ResponseDTO> GetTicketByEmail(string email, int page, int limit);
        Task<ResponseDTO> UpdateTicketImg(List<string> imgList, int ticketId);
    }
}
