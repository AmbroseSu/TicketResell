using System.Security.Claims;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Response;

namespace Service;

public interface IUserService
{
    Task<ClaimsPrincipal> LoadUserByUsernameAsync(string email);
    Task SaveUserVerificationTokenAsync(User user, string token);
    Task<string> ValidateTokenAsync(string theToken, long userId);
    Task<ResponseDTO> GetUserByEmailAsync(string email);
    Task<ResponseDTO> FindAllByRoleAsync(Role role, int page, int limit);
    Task<ResponseDTO> FindAllUsersAsync(int page, int limit);
    Task<ResponseDTO> FindAllCustomersByDateAndYearAsync(int month, int year, int page, int limit);
    Task<ResponseDTO> FindAllNumberOfCustomersByDateAndYearAsync(int month, int year);
    Task<ResponseDTO> FindUserByIdAsync(long userId);
    Task<ResponseDTO> EditProfileAsync(UpsertUserDTO userDTO);
    
}