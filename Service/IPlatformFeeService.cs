using BusinessObject;
using DataAccess.DTO;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

namespace Service;

public interface IPlatformFeeService
{
    Task CreatePlatformFee(PlatformFeeDTO platformFeeDto);
    Task<ResponseDTO> GetAll(int page, int limit);
    Task<ResponseDTO> GetByName(int page, int limit, string? query);
    Task<ResponseDTO> CreatePlatformFee(PlatFormFeeRequest platFormFeeRequest);

}