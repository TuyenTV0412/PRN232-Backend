using Backend.DTO;
using Backend.Model;
using Backend.Repository.Books;
using Backend.Service.Books;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Service.Books
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBook()
        {
            return await _bookRepository.GetAllBook();
        }

        public async Task<Book> GetBookById(int bookId)
        {
            return await _bookRepository.GetBookById(bookId);
        }

        public async Task<List<Book>> GetBookByCategory(int categoryId)
        {
            return await _bookRepository.GetBookByCategory(categoryId);
        }

        public async Task<List<Book>> GetBookByAuthor(int authorId)
        {
            return await _bookRepository.GetBookByAuthor(authorId);
        }

        public async Task<Book> AddBook(Book book)
        {
            return await _bookRepository.AddBook(book);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            return await _bookRepository.UpdateBook(book);
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            return await _bookRepository.DeleteBook(bookId);
        }

        public async Task<List<Book>> GetTop4BookNew()
        {
            return await _bookRepository.GetTop4BookNew();
        }



        public async Task<PagedResult<BookDTO>> GetBookByPages(int? categoryId, string query, int page, int pageSize)
        {
            return await _bookRepository.GetBookByPages(categoryId, query, page, pageSize);
        }

    }
}
