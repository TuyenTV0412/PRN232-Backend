using Backend.DTO;
using Backend.Model;

namespace Backend.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username, string password);

        Task<List<User>> GetAllUsersAsync();

        Task<User> GetUserById(int id);

        Task<bool> UpdateUserRoleAsync(int userId, int roleId);

        Task<User?> GetUserByUsername(string username);

        Task<bool> UpdateUserProfileAsync(int personId, UserUpdateDto dto);

        Task<UserInfoByCardDto?> GetUserInfoByCardIdAsync(int cardId);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> UsernameExistsAsync(string username);

        Task AddUserAsync(User user);

        Task SaveChangesAsync();

        Task<User?> GetByEmailAsync(string email);

        Task<User> CreateUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);

        Task UpdateUserAsync(User user);

    }
}
