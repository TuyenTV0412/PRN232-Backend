using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Authors
{
   


    public class AuthorRepository : IAuthorRepository
    {
        private readonly Prn232Context _prn232Context;

        public AuthorRepository(Prn232Context prn232Context)
        {
            _prn232Context = prn232Context;
        }
        public async Task<List<Author>> GetAllAuthor()
        {
            return await _prn232Context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await _prn232Context.Authors.FirstOrDefaultAsync(a=>a.AuthorId==id);
        }

        public async Task<List<Author>> GetTop4Author()
        {
            var author = await _prn232Context.Authors
                                           .OrderByDescending(b => b.AuthorId) 
                                           .Take(4)
                                           .ToListAsync();
            return author;
        }
    }
}
