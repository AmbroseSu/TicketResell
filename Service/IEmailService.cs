using DataAccess.DTO.Response;

namespace Service;

public interface IEmailService
{
    Task<ResponseDTO> SendEmail(String email);
}