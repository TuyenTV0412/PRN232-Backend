namespace Backend.DTO
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? StartDate { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }

    }

}
