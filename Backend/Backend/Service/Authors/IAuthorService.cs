using Backend.Model;

namespace Backend.Service.Authors
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAuthor();
        Task<Author> GetAuthorById(int id);

        Task<List<Author>> GetTop4Author();
    }
}
