using Backend.Model;

namespace Backend.Repository.Authors
{
    public interface IAuthorRepository
    {

        Task<List<Author>> GetAllAuthor();
        Task<Author> GetAuthorById(int id);

        Task<List<Author>> GetTop4Author();
    }
}
