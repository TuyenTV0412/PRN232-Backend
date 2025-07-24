namespace Backend.DTO
{
    public class BorrowManageItemDto
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public List<BorrowManageBorrowDto> Borrows { get; set; } = new();
    }

    public class BorrowManageBorrowDto
    {
        public int BorrowId { get; set; }
        public string BorrowDate { get; set; } = "";
        public string Deadline { get; set; } = "";
        public string? ReturnDate { get; set; }
        public List<BorrowManageDetailDto> Details { get; set; } = new();
    }

    public class BorrowManageDetailDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = "";
        public int Amount { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = "";
    }

    public class UpdateBorrowStatusDto
    {
        public int BorrowId { get; set; }
        public int StatusId { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }

    public class StatusDto
    {
        public int StatusId { get; set; }

        public string StatusName { get; set; } = null!;
    }

}
