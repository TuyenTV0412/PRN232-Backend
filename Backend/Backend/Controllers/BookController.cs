using Backend.Model;
using Backend.Service.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] int? categoryId, [FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 8)
        {
            var books = await _bookService.GetBookByPages(categoryId, query, page, pageSize);
            return Ok(books);
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBook();
            return Ok(books);
        }

        // GET: api/Book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        //top4 book new
        [HttpGet("Top4Book")]
        public async Task<ActionResult<List<Book>>> GetTop4BookNew()
        {
            var books = await _bookService.GetTop4BookNew();
            return Ok(books);
        }
        // GET: api/Book/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<Book>>> GetBooksByCategory(int categoryId)
        {
            var books = await _bookService.GetBookByCategory(categoryId);
            return Ok(books);
        }

        // GET: api/Book/author/{authorId}
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<Book>>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBookByAuthor(authorId);
            return Ok(books);
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book book)
        {
            var createdBook = await _bookService.AddBook(book);
            // createdBook.Id đã có giá trị tự tăng
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
        }

        // PUT: api/Book/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] Book book)
        {
            if (id != book.BookId)
                return BadRequest();

            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBook(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
