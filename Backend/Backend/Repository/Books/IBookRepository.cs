﻿using Backend.DTO;
using Backend.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repository.Books
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBook();

        Task<PagedResult<BookDTO>> GetBookByPages(int? categoryId, string query, int page = 1, int pageSize = 8);
        Task<BookDTO> GetBookById(int bookId);
        Task<List<Book>> GetBookByCategory(int categoryId);

        Task<List<Book>> GetTop4BookNew();
        Task<List<Book>> GetBookByAuthor(int authorId);

        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(BookUpdateDTO book);
        Task<bool> DeleteBook(int bookId);

        Task<List<Book>> GetTop4BookByCategory(int categoryId);
    }
}
