using Backend.DTO;
using Backend.Model;

namespace Backend.Service.Users
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();

        Task<User> GetUserById(int id);

        Task<bool> UpdateUserRoleAsync(int userId, int roleId);

        Task<User?> GetUserByUsername(string username);

        Task<bool> UpdateUserProfileAsync(int personId, UserUpdateDto dto);

        Task<UserInfoByCardDto?> GetUserInfoByCardIdAsync(int cardId);

        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);

        Task<bool> EmailExistsAsync(string email);

        Task<User> GetOrCreateGoogleUserAsync(Google.Apis.Auth.GoogleJsonWebSignature.Payload googlePayload);

        Task SendForgotPasswordMailAsync(ForgotPasswordRequest req);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest req);

        Task<bool> VerifyOtpAsync(string email, string otp);
    }
}
