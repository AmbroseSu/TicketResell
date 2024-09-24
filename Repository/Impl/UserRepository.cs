using BusinessObject;
using BusinessObject.Enums;
using DataAccess.DAO;

namespace Repository.Impl;

public class UserRepository : IUserRepository
{
    public async Task SaveAsync(User user) => await UserDAO.Instance.SaveAsync(user);

    public async Task UpdateAsync(User user) => await UserDAO.Instance.UpdateAsync(user);

    public async Task DeleteAsync(long userId) => await UserDAO.Instance.DeleteAsync(userId);
    
    public async Task<User?> FindByLoginAsync(string login) => await UserDAO.Instance.FindByLoginAsync(login);

    public async Task<User?> FindByPhoneAsync(string phone) => await UserDAO.Instance.FindByPhoneAsync(phone);

    public async Task<bool> ExistsByEmailAsync(string email) => await UserDAO.Instance.ExistsByEmailAsync(email);

    public async Task<bool> ExistsByPhoneAsync(string phone) => await UserDAO.Instance.ExistsByPhoneAsync(phone);

    public async Task<User?> FindByRoleAsync(Role role) => await UserDAO.Instance.FindByRoleAsync(role);

    public async Task<User?> FindUserByEmailAsync(string email) => await UserDAO.Instance.FindUserByEmailAsync(email);

    public async Task<User?> FindUserByIdAsync(long id) => await UserDAO.Instance.FindUserByIdAsync(id); 

    public async Task<User?> FindUserByPhoneAsync(string phone) => await UserDAO.Instance.FindUserByPhoneAsync(phone);
}