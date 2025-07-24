using Backend.Model;

namespace Backend.DTO
{
    public class UserDTO
    {
        public int PersonId { get; set; }

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string Username { get; set; } = null!;

        public int RoleId { get; set; }

        public string RoleName { get; set; }

    }
}
