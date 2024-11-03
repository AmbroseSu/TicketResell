using System.Net;
using System.Security.Claims;
using AutoMapper;
using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DTO;
using DataAccess.DTO.Response;
using Repository;
using Service.Response;

namespace Service.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IVerificationTokenRepository _tokenRepository;
    private readonly IPostRepository _postRepository;

    public UserService(
        IUserRepository userRepository,
        IVerificationTokenRepository tokenRepository, IMapper mapper, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
        _postRepository = postRepository;
    }

    public async Task<ClaimsPrincipal> LoadUserByUsernameAsync(string email)
    {
        var user = await _userRepository.FindByLoginAsync(email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
            // Thêm các claim khác nếu cần
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CustomAuthType");
        return new ClaimsPrincipal(claimsIdentity);
    }

    public async Task SaveUserVerificationTokenAsync(User user, string token)
    {
        var verificationToken = new VerificationToken(token, user.Id);
        await _tokenRepository.SaveAsync(verificationToken);
    }

    public async Task<string> ValidateTokenAsync(string theToken, long userId)
    {
        var token = await _tokenRepository.FindByTokenAsync(theToken);
        if (token == null || token.User.Id != userId)
        {
            return "Invalid verification token";
        }

        var user = token.User;
        var currentTime = DateTime.Now;
        var tokenExpirationTime = token.ExpirationTime;

        if (tokenExpirationTime <= currentTime)
        {
            await _tokenRepository.DeleteAsync(token.Id);
            return "Token already expired";
        }

        user.IsEnabled = true; // Assuming user has an IsEnabled property
        await _userRepository.SaveAsync(user);
        await _tokenRepository.DeleteAsync(token.Id);
        return "Valid";
    }


    public async Task<ResponseDTO> GetUserByEmailAsync(string email)
    {
        try
        {
            User? user = await _userRepository.FindUserByEmailAsync(email.ToLower());
            UserDTO userDto = _mapper.Map<UserDTO>(user);
                
            return ResponseUtil.GetObject(userDto, "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> FindAllByRoleAsync(Role role,int page, int limit)
    {
        try
        {
            IEnumerable<User?> users = await _userRepository.FindAllByRoleAsync(role);
            IEnumerable<UserDTO> userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            List<UserDTO> result = userDtos.Skip((page - 1) * limit).Take(limit).ToList();
            //UserDTO userDto = _mapper.Map<UserDTO>(user);
                
            return ResponseUtil.GetCollection(result, "ok", HttpStatusCode.Created, page, limit, users.Count());
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> FindAllUsersAsync(int page, int limit)
    {
        try
        {
            IEnumerable<User?> users = await _userRepository.FindAllUsersAsync();
            IEnumerable<UserDTO> userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            List<UserDTO> result = userDtos.Skip((page - 1) * limit).Take(limit).ToList();
            //UserDTO userDto = _mapper.Map<UserDTO>(user);
                
            return ResponseUtil.GetCollection(result, "ok", HttpStatusCode.Created, page, limit, users.Count());
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> FindAllCustomersByDateAndYearAsync(int month, int year, int page, int limit)
    {
        try
        {
            IEnumerable<User?> users = await _userRepository.FindAllCustomersByDateAndYearAsync(month, year);
            IEnumerable<UserDTO> userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            List<UserDTO> result = userDtos.Skip((page - 1) * limit).Take(limit).ToList();
            //UserDTO userDto = _mapper.Map<UserDTO>(user);
                
            return ResponseUtil.GetCollection(result, "ok", HttpStatusCode.Created, page, limit, users.Count());
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> FindAllNumberOfCustomersByDateAndYearAsync(int month, int year)
    {
        try
        {
            IEnumerable<User?> users = await _userRepository.FindAllCustomersByDateAndYearAsync(month, year);
            return ResponseUtil.GetObject(users.Count(), "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> FindUserByIdAsync(long userId)
    {
        try
        {
            User? user = await _userRepository.FindUserByIdAsync(userId);
            if (user is null)
            {
                return ResponseUtil.Error("User not found", "Faild", HttpStatusCode.NotFound);
            }
            var result = _mapper.Map<UserDTO>(user);
            return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> EditProfileAsync(UpsertUserDTO userDTO)
    {
        try
        {
            User? user = await _userRepository.FindUserByIdAsync(userDTO.Id);
            if (user is null)
            {
                return ResponseUtil.Error("User not found", "Faild", HttpStatusCode.NotFound);
            }
            var fields = typeof(UpsertUserDTO).GetProperties();
            foreach (var field in fields)
            {
                // Bỏ qua các thuộc tính cụ thể không muốn cập nhật
                if (field.Name == "Id" || field.Name == "Email" || field.Name == "Role" || field.Name == "Gender")
                {
                    continue;
                }

                // Lấy giá trị mới từ userDTO
                var newValue = field.GetValue(userDTO);
                if (newValue != null)
                {
                    // Tìm thuộc tính tương ứng trong lớp User
                    var userField = typeof(User).GetProperty(field.Name);
                    if (userField != null && userField.CanWrite)
                    {
                        // Gán giá trị mới cho thuộc tính của user
                        userField.SetValue(user, newValue);
                    }
                }
            }
            
            await _userRepository.UpdateAsync(user);
            var result = _mapper.Map<UserDTO>(user);
            return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDTO> GetUserByTicketIdAsync(long ticketId)
    {
        try
        {
            IEnumerable<Post?> result = await _postRepository.Find(c => c.IsDeleted == false && c.TicketId == ticketId);
            //var result = _mapper.Map<UserDTO>(user);
            return ResponseUtil.GetObject(result, "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
    }
}