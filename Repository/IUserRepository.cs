using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DAO;

namespace Repository;

public interface IUserRepository
{
    Task SaveAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(long userId);
    Task<User?> FindByLoginAsync(string login);
    Task<User?> FindByPhoneAsync(string phone);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByPhoneAsync(string phone);
    Task<User?> FindByRoleAsync(Role role);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByIdAsync(long id);
    Task<User?> FindUserByPhoneAsync(string phone);
    Task<List<User>> FindAllByRoleAsync(Role role);
    Task<List<User>> FindAllUsersAsync();
    Task<List<User>> FindAllCustomersByDateAndYearAsync(int month, int year);
    Task<List<User>> SearchUsersByEmailAndFullNameAsync(string search);

}