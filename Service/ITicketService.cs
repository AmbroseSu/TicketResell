using DataAccess.DTO;
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
        Task<ResponseDTO> CreateTicketAsync(TicketDTO ticket);
        Task<ResponseDTO> UpdateTicketAsync(TicketDTO ticket);
        Task<ResponseDTO> DeleteTicketAsync(int id);
    }
}
