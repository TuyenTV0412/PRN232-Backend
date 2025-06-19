namespace Backend.DTO
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
    }

}
