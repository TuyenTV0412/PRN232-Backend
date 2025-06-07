using Backend.Model;

namespace Backend.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username, string password);
    }
}
