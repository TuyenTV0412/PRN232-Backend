namespace Backend.DTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string? Image { get; set; }
        public string? Hometown { get; set; }
        // Thêm các trường khác nếu cần
    }

    public class PagedAuthorsResultDto
    {
        public List<AuthorDto> Data { get; set; } = new();
        public int TotalPages { get; set; }
    }

}
