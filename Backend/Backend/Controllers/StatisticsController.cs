using Backend.DTO;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly Prn232Context _context; // Đổi thành DbContext của bạn

        public StatisticsController(Prn232Context context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var now = DateTime.Today;

            var totalBookQuantity = await _context.Books.SumAsync(b => (int?)b.Quantity ?? 0);
            var totalBookTitles = await _context.Books.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalAuthors = await _context.Authors.CountAsync();
            var totalReaders = await _context.Users.CountAsync(u => u.RoleId == 1);


            var totalBorrows = await _context.Borrows.CountAsync();
            var totalReturned = await _context.BorrowDetails.CountAsync(d => d.StatusId == 2);
            var totalBorrowing = await _context.BorrowDetails.CountAsync(d => d.StatusId == 1);


            var topBooks = await _context.BorrowDetails
                .GroupBy(d => d.Book.BookName)
                .Select(g => new { Name = g.Key, Count = g.Sum(x => x.Amount) })
                .OrderByDescending(g => g.Count).Take(5)
                .Select(x => x.Name).ToListAsync();

            var topReaders = await _context.Borrows
                .GroupBy(b => b.Person.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count).Take(5)
                .Select(x => x.Name).ToListAsync();

            var result = new LibraryStatisticsDto
            {
                TotalBookQuantity = totalBookQuantity,
                TotalBookTitles = totalBookTitles,
                TotalCategories = totalCategories,
                TotalAuthors = totalAuthors,
                TotalReaders = totalReaders,
                TotalBorrows = totalBorrows,
                TotalReturned = totalReturned,
                TotalBorrowing = totalBorrowing,
                TopBooks = topBooks,
                TopReaders = topReaders
            };

            return Ok(result);
        }
    }

}
