using Backend.Model;
using Backend.Repository.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repository.Books
{
    public class BookRepository : IBookRepository
    {
        private readonly Prn232Context _context;

        public BookRepository(Prn232Context context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBook()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookById(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task<List<Book>> GetBookByCategory(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Book>> GetBookByAuthor(int authorId)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return false;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Book>> GetTop4BookNew()
        {
            var newestBooks = await _context.Books
                                                 .Include(b => b.Author)
                                                 .Include(b => b.Category)
                                                 .Include(b => b.Publisher)
                                                 .OrderByDescending(b => b.BookId)
                                                 .Take(4)
                                                 .ToListAsync();
            return newestBooks;
        }
    }
}
