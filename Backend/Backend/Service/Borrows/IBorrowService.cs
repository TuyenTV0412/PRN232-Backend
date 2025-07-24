using Backend.DTO;
using Backend.Model;

namespace Backend.Service.Borrows
{
    public interface IBorrowService
    {
        Task<(bool Success, string Message)> CheckoutBooksAsync(BorrowRequestDTO request);

        Task<List<BorrowResponseDto>> GetBorrowsByPersonIdAsync(int personId);
        Task<BorrowResponseDto?> GetBorrowDetailByBorrowIdAsync(int borrowId);

        Task<List<BorrowManageItemDto>> GetManageBorrows(DateOnly? deadlineDate = null);
        Task<bool> RemoveBorrowDetail(int borrowId, int bookId);
        Task<bool> RemoveBorrow(int borrowId);
        Task<bool> RemoveAllBorrows(int personId);

        Task<bool> UpdateBorrowStatusAsync(UpdateBorrowStatusDto dto);

        Task<List<StatusDto>> GetAllStatusesAsync();
    }
}
