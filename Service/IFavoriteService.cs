using DataAccess.DTO.Response;

namespace Service;

public interface IFavoriteService
{
    Task<ResponseDTO> AddTicketFavorite(int userId, int ticketId);
    Task<ResponseDTO> GetAllFavoriteTicketByUserId(int userId);
    Task<ResponseDTO> RemoveFavoriteTicket(int userId, int ticketId);

}