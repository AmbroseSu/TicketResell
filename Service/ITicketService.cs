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
        Task<ResponseDTO> CreateTicketAsync(NewTicketRequest ticket);
        Task<ResponseDTO> UpdateTicketAsync(updateTicketRequest ticket);
        Task<ResponseDTO> DeleteTicketAsync(int id);
        Task<ResponseDTO> GetTicketAsync(int id);
        Task<ResponseDTO> GetTicketsAsync(int page, int limit);
        Task<ResponseDTO> updateStatus(int id, string status);
    }
}
