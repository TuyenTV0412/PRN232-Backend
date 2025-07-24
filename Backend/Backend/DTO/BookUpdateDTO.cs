namespace Backend.DTO
{
    public class BookUpdateDTO
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? Images { get; set; }
        public string? Description { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
    }

}
