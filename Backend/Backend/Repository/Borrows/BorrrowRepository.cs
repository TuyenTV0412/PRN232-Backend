using Backend.DTO;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Repository.Borrows
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly Prn232Context _context;
        private IDbContextTransaction? _transaction;

        public BorrowRepository(Prn232Context context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsername(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<List<Book>> GetBooksByIds(List<int> bookIds)
            => await _context.Books.Where(b => bookIds.Contains(b.BookId)).ToListAsync();

        public async Task AddBorrow(Borrow borrow)
            => await _context.Borrows.AddAsync(borrow);

        public async Task AddBorrowDetails(List<BorrowDetail> details)
            => await _context.BorrowDetails.AddRangeAsync(details);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task<List<BorrowResponseDto>> GetBorrowsByPersonIdAsync(int personId)
        {
            var borrows = await _context.Borrows
                .Where(b => b.PersonId == personId)
                .Include(b => b.BorrowDetails)
                    .ThenInclude(d => d.Book)
                .Include(b => b.BorrowDetails)
                    .ThenInclude(d => d.Status) // ✅ Include status for StatusName
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return borrows.Select(b => new BorrowResponseDto
            {
                BorrowId = b.BorrowId,
                BorrowDate = b.BorrowDate,
                Deadline = b.Deadline,
                Details = b.BorrowDetails.Select(d => new BorrowDetailDto
                {
                    BookId = d.BookId,
                    BookName = d.Book?.BookName ?? "",
                    Amount = d.Amount,
                    StatusId = d.Status.StatusId,
                    StatusName = d.Status?.StatusName ?? "Không xác định" // ✅ Fetch status name safely
                }).ToList()
            }).ToList();
        }



public async Task<BorrowResponseDto?> GetBorrowDetailByBorrowIdAsync(int borrowId)
{
    var borrow = await _context.Borrows
        .Include(b => b.BorrowDetails)
            .ThenInclude(d => d.Book)
        .Include(b => b.BorrowDetails)
            .ThenInclude(d => d.Status)
        .FirstOrDefaultAsync(b => b.BorrowId == borrowId);

    if (borrow == null) return null;

    // Chắc chắn có ít nhất một BorrowDetail
    var firstDetail = borrow.BorrowDetails.FirstOrDefault();
    int statusId = firstDetail?.StatusId ?? 0;
    string statusName = firstDetail?.Status?.StatusName ?? "";

    return new BorrowResponseDto
    {
        BorrowId = borrow.BorrowId,
        BorrowDate = borrow.BorrowDate,
        Deadline = borrow.Deadline,
        ReturnDate = borrow.ReturnDate,
        StatusId = statusId,
        StatusName = statusName,
        Details = borrow.BorrowDetails.Select(d => new BorrowDetailDto
        {
            BookId = d.BookId,
            BookName = d.Book?.BookName ?? "",
            Amount = d.Amount,
            StatusId = statusId,
            StatusName = statusName,
        }).ToList()
    };
}


        public async Task<List<BorrowManageItemDto>> GetManageBorrows(DateOnly? deadlineDate = null)
        {
            var query = _context.Borrows
                .Include(b => b.Person)
                .Include(b => b.BorrowDetails)
                    .ThenInclude(d => d.Book)
                .Include(b => b.BorrowDetails)
                    .ThenInclude(d => d.Status)
                .AsQueryable();

            if (deadlineDate.HasValue)
                query = query.Where(b => b.Deadline == deadlineDate.Value);

            var borrows = await query.ToListAsync();

            var grouped = borrows
                .GroupBy(b => b.PersonId)
                .Select(g => new BorrowManageItemDto
                {
                    PersonId = g.Key,
                    PersonName = g.First().Person.Name,
                    Borrows = g.Select(b => new BorrowManageBorrowDto
                    {
                        BorrowId = b.BorrowId,
                        BorrowDate = b.BorrowDate.ToString("yyyy-MM-dd"),
                        Deadline = b.Deadline.ToString("yyyy-MM-dd"),
                        ReturnDate = b.ReturnDate.HasValue ? b.ReturnDate.Value.ToString("yyyy-MM-dd") : null,
                        Details = b.BorrowDetails.Select(d => new BorrowManageDetailDto
                        {
                            BookId = d.BookId,
                            BookName = d.Book.BookName,
                            Amount = d.Amount,
                            StatusId = d.Status.StatusId,
                            StatusName = d.Status.StatusName
                        }).ToList()
                    }).ToList()
                }).ToList();

            return grouped;
        }

        public async Task<bool> RemoveBorrowDetail(int borrowId, int bookId)
        {
            var detail = await _context.BorrowDetails.FirstOrDefaultAsync(bd => bd.BorrowId == borrowId && bd.BookId == bookId);
            if (detail == null) return false;
            _context.BorrowDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveBorrow(int borrowId)
        {
            var details = await _context.BorrowDetails.Where(bd => bd.BorrowId == borrowId).ToListAsync();
            if (details.Any())
                _context.BorrowDetails.RemoveRange(details);

            var borrow = await _context.Borrows.FindAsync(borrowId);
            if (borrow != null)
                _context.Borrows.Remove(borrow);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAllBorrows(int personId)
        {
            var borrows = await _context.Borrows.Where(b => b.PersonId == personId).ToListAsync();
            foreach (var borrow in borrows)
            {
                var details = await _context.BorrowDetails.Where(d => d.BorrowId == borrow.BorrowId).ToListAsync();
                if (details.Any())
                    _context.BorrowDetails.RemoveRange(details);
            }
            if (borrows.Any()) _context.Borrows.RemoveRange(borrows);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId)
        {
            return await _context.Borrows
                .Include(b => b.BorrowDetails)
                .ThenInclude(bd => bd.Book)
                .FirstOrDefaultAsync(b => b.BorrowId == borrowId);
        }

        public async Task UpdateBorrowAsync(Borrow borrow)
        {
            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StatusDto>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Select(s => new StatusDto
                {
                    StatusId = s.StatusId, // hoặc s.Id tùy entity
                    StatusName = s.StatusName
                })
                .ToListAsync();
        }

        public async Task<User?> GetUserByPersonid(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PersonId == id);
        }
    }
}
