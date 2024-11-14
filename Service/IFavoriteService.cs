using DataAccess.DTO.Response;

namespace Repository;

public interface IFavoriteService
{
    Task<ResponseDTO> AddTicketFavorite(int userId, int ticketId);
    Task<ResponseDTO> GetAllFavoriteTicketByUserId(int userId);
    Task<ResponseDTO> RemoveFavoriteTicket(int cartItemId);
}