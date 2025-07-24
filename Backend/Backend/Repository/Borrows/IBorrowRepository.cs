using Backend.DTO;
using Backend.Model;

namespace Backend.Repository.Borrows
{
    public interface IBorrowRepository
    {
        Task<User?> GetUserByUsername(string username);

        Task<User?> GetUserByPersonid(int id);
        Task<List<Book>> GetBooksByIds(List<int> bookIds);
        Task AddBorrow(Borrow borrow);
        Task AddBorrowDetails(List<BorrowDetail> details);
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        Task<List<BorrowResponseDto>> GetBorrowsByPersonIdAsync(int personId);
        Task<BorrowResponseDto?> GetBorrowDetailByBorrowIdAsync(int borrowId);


        Task<List<BorrowManageItemDto>> GetManageBorrows(DateOnly? deadlineDate = null);
        Task<bool> RemoveBorrowDetail(int borrowId, int bookId);
        Task<bool> RemoveBorrow(int borrowId);
        Task<bool> RemoveAllBorrows(int personId);

        Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId);
        Task UpdateBorrowAsync(Borrow borrow);

        Task<List<StatusDto>> GetAllStatusesAsync();

    }
}
