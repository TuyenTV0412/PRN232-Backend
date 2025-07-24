namespace Backend.DTO
{
    public class BorrowResponseDto
    {
        public int BorrowId { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly Deadline { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; } = string.Empty;
        public List<BorrowDetailDto> Details { get; set; } = new();
    }


}
