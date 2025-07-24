using Backend.DTO;
using Backend.Model;

namespace Backend.Service.Authors
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAuthor();
        Task<Author?> GetAuthorById(int id);
        Task<List<Author>> GetTop4Author();
        Task<Author> AddAuthor(Author author);
        Task<Author?> UpdateAuthor(Author author);
        Task<bool> DeleteAuthor(int id);

        Task<PagedAuthorsResultDto> GetAuthorsAsync(int page, int pageSize, string? search);
    }
}
