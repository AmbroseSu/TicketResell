using DataAccess.DTO.Response;

namespace Service;

public interface ITransactionService
{
    Task<ResponseDTO> GetAllByuserId(int userId);
}