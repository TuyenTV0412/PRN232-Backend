using Backend.Model;
using Backend.Repository.Authors;

namespace Backend.Service.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository) { 
                        _authorRepository = authorRepository;
        }
        public async Task<List<Author>> GetAllAuthor()
        {
            return await _authorRepository.GetAllAuthor();
        }

        public async Task<Author> GetAuthorById(int id)
        {
           return await _authorRepository.GetAuthorById(id);
        }

        public async Task<List<Author>> GetTop4Author()
        {
           return await _authorRepository.GetTop4Author();
        }
    }
}
