namespace Backend.DTO
{
    public class LibraryStatisticsDto
    {
        public int TotalBookQuantity { get; set; }
        public int TotalBookTitles { get; set; }
        public int TotalCategories { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalReaders { get; set; }
        public int TotalBorrows { get; set; }
        public int TotalReturned { get; set; }
        public int TotalBorrowing { get; set; }
        public List<string> TopBooks { get; set; } = new();
        public List<string> TopReaders { get; set; } = new();
    }

}
