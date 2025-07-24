using Backend.DTO;
using Backend.Model;
using Backend.Repository.Borrows;

namespace Backend.Service.Borrows
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;

        public BorrowService(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }

        public async Task<(bool Success, string Message)> CheckoutBooksAsync(BorrowRequestDTO request)
        {
            await _borrowRepository.BeginTransactionAsync();
            try
            {
                var user = !string.IsNullOrEmpty(request.Username)
     ? await _borrowRepository.GetUserByUsername(request.Username)
     : await _borrowRepository.GetUserByPersonid(request.PersonId.Value);

                if (user == null)
                    return (false, "Không tìm thấy người dùng");


                var books = await _borrowRepository.GetBooksByIds(request.BookIds);
                var errorMessages = new List<string>();
                var booksToUpdate = new List<Book>();

                foreach (var bookId in request.BookIds)
                {
                    var book = books.FirstOrDefault(b => b.BookId == bookId);
                    if (book == null)
                        errorMessages.Add($"Không tìm thấy sách ID {bookId}");
                    else if (book.Quantity < 1)
                        errorMessages.Add($"Sách '{book.BookName}' hiện đã mượn hết, vui lòng mượn lại sau!");
                    else
                        booksToUpdate.Add(book);
                }

                if (errorMessages.Count > 0)
                    return (false, string.Join(" | ", errorMessages));

                var borrow = new Borrow
                {
                    PersonId = user.PersonId,
                    BorrowDate = DateOnly.Parse(request.ValidFrom),
                    Deadline = DateOnly.Parse(request.ValidThru)
                };
                await _borrowRepository.AddBorrow(borrow);
                await _borrowRepository.SaveChangesAsync();

                var borrowDetails = new List<BorrowDetail>();
                foreach (var book in booksToUpdate)
                {
                    borrowDetails.Add(new BorrowDetail
                    {
                        BorrowId = borrow.BorrowId,
                        BookId = book.BookId,
                        Amount = 1,
                        StatusId = 1
                    });
                    book.Quantity -= 1;
                }
                await _borrowRepository.AddBorrowDetails(borrowDetails);
                await _borrowRepository.SaveChangesAsync();
                await _borrowRepository.CommitTransactionAsync();

                return (true, "Mượn sách thành công!");
            }
            catch (Exception ex)
            {
                await _borrowRepository.RollbackTransactionAsync();
                return (false, $"Lỗi khi xử lý: {ex.Message}");
            }
        }

        public async Task<BorrowResponseDto?> GetBorrowDetailByBorrowIdAsync(int borrowId)
        {
            return await _borrowRepository.GetBorrowDetailByBorrowIdAsync(borrowId);
        }

        public async Task<List<BorrowResponseDto>> GetBorrowsByPersonIdAsync(int personId)
        {
            return await _borrowRepository.GetBorrowsByPersonIdAsync(personId);
        }

        public Task<List<BorrowManageItemDto>> GetManageBorrows(DateOnly? deadlineDate = null)
      => _borrowRepository.GetManageBorrows(deadlineDate);

        public Task<bool> RemoveBorrowDetail(int borrowId, int bookId)
            => _borrowRepository.RemoveBorrowDetail(borrowId, bookId);

        public Task<bool> RemoveBorrow(int borrowId)
            => _borrowRepository.RemoveBorrow(borrowId);

        public Task<bool> RemoveAllBorrows(int personId)
            => _borrowRepository.RemoveAllBorrows(personId);

        public async Task<bool> UpdateBorrowStatusAsync(UpdateBorrowStatusDto dto)
        {
            var borrow = await _borrowRepository.GetBorrowWithDetailsAsync(dto.BorrowId);
            if (borrow == null) return false;

            // Cập nhật ngày trả thực tế nếu truyền lên
            if (dto.ReturnDate.HasValue)
                borrow.ReturnDate = dto.ReturnDate.Value;

            foreach (var detail in borrow.BorrowDetails)
            {
                // Nếu chuyển sang "Đã trả" và trạng thái cũ khác 2 thì cộng lại sách
                if (dto.StatusId == 2 && detail.StatusId != 2)
                {
                    detail.Book.Quantity += detail.Amount;
                }
                detail.StatusId = dto.StatusId;
            }

            await _borrowRepository.UpdateBorrowAsync(borrow);
            return true;
        }

        public Task<List<StatusDto>> GetAllStatusesAsync()
        {
            return _borrowRepository.GetAllStatusesAsync();
        }
    }
}
