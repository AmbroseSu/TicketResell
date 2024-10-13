using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

namespace Service;

public interface ITicketRequestService
{
    Task<ResponseDTO> CreateTicketRequestAsync(RequestTicket requestTicket);
    Task UpdateAsync(TicketRequest ticketRequest);
    Task DeleteAsync(long requestId);
    Task<TicketRequest?> FindByIdAsync(long id);
    
    Task<List<ResponseDTO>> FindAllTicketRequestsByTicketIdAsync(int ticketId);
}