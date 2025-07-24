using Backend.DTO;
using Backend.Model;
using Backend.Repository.Authors;

namespace Backend.Service.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<List<Author>> GetAllAuthor() => _authorRepository.GetAllAuthor();

        public Task<Author?> GetAuthorById(int id) => _authorRepository.GetAuthorById(id);

        public Task<List<Author>> GetTop4Author() => _authorRepository.GetTop4Author();

        public Task<Author> AddAuthor(Author author) => _authorRepository.AddAuthor(author);

        public Task<Author?> UpdateAuthor(Author author) => _authorRepository.UpdateAuthor(author);

        public Task<bool> DeleteAuthor(int id) => _authorRepository.DeleteAuthor(id);

        public Task<PagedAuthorsResultDto> GetAuthorsAsync(int page, int pageSize, string? search)
      => _authorRepository.GetAuthorsAsync(page, pageSize, search);
    }

}
