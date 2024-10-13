using BusinessObject;

namespace Repository;

public interface ITicketRequestRepository
{
    Task SaveAsync(TicketRequest ticketRequest);
    Task UpdateAsync(TicketRequest ticketRequest);
    Task DeleteAsync(long requestId);
    Task<TicketRequest?> FindByIdAsync(long id);
    Task<List<TicketRequest>> FindAllTicketRequestsByTicketIdAsync(int ticketId);
}