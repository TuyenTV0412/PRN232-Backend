namespace Backend.DTO
{
    public class BorrowDetailDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public int? StatusId { get; set; }

          public string? StatusName { get; set; } = string.Empty;
    }
    public class BorrowDetailDto1
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}
