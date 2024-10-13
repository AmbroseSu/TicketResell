using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class TicketRequestRepository : ITicketRequestRepository
{
    public Task SaveAsync(TicketRequest ticketRequest) => TicketRequestDAO.Instance.SaveAsync(ticketRequest);

    public Task UpdateAsync(TicketRequest ticketRequest) => TicketRequestDAO.Instance.UpdateAsync(ticketRequest);

    public Task DeleteAsync(long requestId) => TicketRequestDAO.Instance.DeleteAsync(requestId);

    public Task<TicketRequest?> FindByIdAsync(long id) => TicketRequestDAO.Instance.FindByIdAsync(id);
    
    public Task<List<TicketRequest>> FindAllTicketRequestsByTicketIdAsync(int ticketId) => TicketRequestDAO.Instance.FindAllTicketRequestsByTicketIdAsync(ticketId);
}