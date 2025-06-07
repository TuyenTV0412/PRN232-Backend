using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly Prn232Context _context;

        public UserRepository(Prn232Context context)
        {
            _context = context;
        }
        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _context.Users
    .Include(u => u.Role)
    .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
             
        }
    }
}
