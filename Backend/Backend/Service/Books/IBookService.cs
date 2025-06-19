using Backend.DTO;
using Backend.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Service.Books
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBook();

        Task<PagedResult<BookDTO>> GetBookByPages(int? categoryId, string query, int page = 1, int pageSize = 8 );
        Task<Book> GetBookById(int bookId);
        Task<List<Book>> GetBookByCategory(int categoryId);
        Task<List<Book>> GetBookByAuthor(int authorId);
        Task<List<Book>> GetTop4BookNew();
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(int bookId);
    }
}
