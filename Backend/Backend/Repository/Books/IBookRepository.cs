using Backend.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repository.Books
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBook();
        Task<Book> GetBookById(int bookId);
        Task<List<Book>> GetBookByCategory(int categoryId);

        Task<List<Book>> GetTop4BookNew();
        Task<List<Book>> GetBookByAuthor(int authorId);

        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(int bookId);
    }
}
