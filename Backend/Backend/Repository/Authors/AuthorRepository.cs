using Backend.DTO;
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
        => await _prn232Context.Authors.ToListAsync();

        public async Task<Author?> GetAuthorById(int id)
            => await _prn232Context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);

        public async Task<List<Author>> GetTop4Author()
            => await _prn232Context.Authors.OrderByDescending(b => b.AuthorId).Take(4).ToListAsync();

        public async Task<Author> AddAuthor(Author author)
        {
            _prn232Context.Authors.Add(author);
            await _prn232Context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAuthor(Author author)
        {
            var existing = await _prn232Context.Authors.FindAsync(author.AuthorId);
            if (existing == null) return null;

            existing.AuthorName = author.AuthorName;
            existing.Hometown = author.Hometown;
            existing.DateOfBirth = author.DateOfBirth;
            existing.DateOfDeath = author.DateOfDeath;
            existing.Image = author.Image;

            await _prn232Context.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _prn232Context.Authors.FindAsync(id);
            if (author == null) return false;
            _prn232Context.Authors.Remove(author);
            await _prn232Context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedAuthorsResultDto> GetAuthorsAsync(int page, int pageSize, string? search)
        {
            var query = _prn232Context.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchKW = search.Trim().ToLower();
                query = query.Where(a => a.AuthorName.ToLower().Contains(searchKW));
            }

            int totalAuthors = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalAuthors / pageSize);

            var authors = await query
                .OrderBy(a => a.AuthorName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorDto
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorName,
                    Image = a.Image,
                    Hometown = a.Hometown
                })
                .ToListAsync();

            return new PagedAuthorsResultDto
            {
                Data = authors,
                TotalPages = totalPages
            };
        }
    }
}
