using Backend.DTO;
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.RoleId != 3)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.PersonId == id);
        }

        public async Task<bool> UpdateUserRoleAsync(int personId, int roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PersonId == personId);
            if (user == null) return false;
            user.RoleId = roleId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UpdateUserProfileAsync(int personId, UserUpdateDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PersonId == personId);
            if (user == null) return false;
            user.Name = dto.Name;
            user.Gender = dto.Gender;
            user.DateOfBirth = dto.DateOfBirth;
            user.Address = dto.Address;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserInfoByCardDto?> GetUserInfoByCardIdAsync(int cardId)
        {
            var card = await _context.Cards
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.CardId == cardId);
            if (card == null || card.Person == null) return null;
            var p = card.Person;
            return new UserInfoByCardDto
            {
                PersonId = p.PersonId,
                Name = p.Name,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Email = p.Email,
                Address = p.Address,
                Phone = p.Phone,
                CardId = card.CardId
            };
        }

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<bool> UsernameExistsAsync(string username) =>
            await _context.Users.AnyAsync(u => u.Username == username);

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
