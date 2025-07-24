namespace Backend.DTO
{
    public class BorrowRequestDTO
    {

        public int? PersonId { get; set; }
        public string? Username { get; set; }
        public List<int> BookIds { get; set; } = new();
        public string ValidFrom { get; set; } = string.Empty;
        public string ValidThru { get; set; } = string.Empty;
    }
}
