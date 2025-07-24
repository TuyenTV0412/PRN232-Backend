using Backend.DTO;
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

        public async Task<BookDTO?> GetBookById(int bookId)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.BookId == bookId)
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    Images = b.Images,
                    Description = b.Description,
                    AuthorId = b.AuthorId,
                    PublisherId = b.PublisherId,
                    CategoryId = b.CategoryId,
                    PublishingYear = b.PublishingYear,
                    AuthorName = b.Author != null ? b.Author.AuthorName : "Không rõ",
                    CategoryName = b.Category != null ? b.Category.CategoryName : "Không rõ",
                    PublisherName = b.Publisher != null ? b.Publisher.PublisherName : "Không rõ",
                    Quantity = b.Quantity
                })
                .FirstOrDefaultAsync();

            return book;
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

        public async Task<Book> UpdateBook(BookUpdateDTO dto)
        {
            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) throw new Exception("Không tìm thấy sách");

            if (dto.BookName != null) book.BookName = dto.BookName;
            if (dto.Images != null) book.Images = dto.Images;
            if (dto.Description != null) book.Description = dto.Description;
            if (dto.AuthorId.HasValue) book.AuthorId = dto.AuthorId.Value;
            if (dto.PublisherId.HasValue) book.PublisherId = dto.PublisherId.Value;
            if (dto.CategoryId.HasValue) book.CategoryId = dto.CategoryId.Value;
            if (dto.Quantity.HasValue) book.Quantity = dto.Quantity.Value;

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

        public async Task<PagedResult<BookDTO>> GetBookByPages(int? categoryId, string query, int page = 1, int pageSize = 8)
        {
            var booksQuery = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                booksQuery = booksQuery.Where(b => b.BookName.Contains(query));
            }
            if (categoryId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            var totalItems = await booksQuery.CountAsync(); // Tổng số sách phù hợp
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var books = await booksQuery
                .OrderBy(b => b.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    Images = b.Images,
                    AuthorId = b.AuthorId,
                    PublisherId = b.PublisherId,
                    CategoryId = b.CategoryId,
                    AuthorName = b.Author.AuthorName,
                    CategoryName = b.Category.CategoryName,
                    PublisherName = b.Publisher.PublisherName,
                    Quantity = b.Quantity,
                    PublishingYear = b.PublishingYear,
                    Description = b.Description
                })
                .ToListAsync();

            return new PagedResult<BookDTO>
            {
                Items = books,
                TotalPages = totalPages,
                CurrentPage = page,
                TotalItems = totalItems
            };
        }

        public async Task<List<Book>> GetTop4BookByCategory(int categoryId)
        {
           var books = await _context.Books
                                                 .Include(b => b.Author)
                                                 .Include(b => b.Category)
                                                 .Include(b => b.Publisher)
                                                 .Where(b => b.CategoryId == categoryId)
                                                 .Take(4)
                                                 .ToListAsync();
            return books;
        }
    }
}
