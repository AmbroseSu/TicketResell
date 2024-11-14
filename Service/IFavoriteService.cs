using DataAccess.DTO.Response;

namespace Repository;

public interface IFavoriteService
{
    Task<ResponseDTO> AddTicketFavorite(int userId, int ticketId);
}