using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

namespace Service;

public interface ITicketRequestService
{
    Task<ResponseDTO> CreateTicketRequestAsync(RequestTicket requestTicket);
    Task<ResponseDTO> AcceptTicketRequestAsync(long requestId);
    Task DeleteAsync(long requestId);
    Task<ResponseDTO> FindByIdAsync(long id);
    Task<ResponseDTO> FindAllTicketRequestsByTicketIdAsync(int ticketId, int page, int limit);
    Task<ResponseDTO> FindAllTicketRequestsByUserIdAsync(int userId, int page, int limit);

}