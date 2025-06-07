namespace Backend.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }

        public string BookName { get; set; } = null!;

        public string? Images { get; set; }

        public int AuthorId { get; set; }

        public int PublisherId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public string PublisherName { get; set; } = null!;

    }
}
