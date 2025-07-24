using Backend.DTO;
using Backend.Model;
using Backend.Repository.Users;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.Service.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;

        private readonly EmailService _emailService;

        private readonly IMemoryCache _cache;

        public UserService(IUserRepository repository, EmailService emailService, IMemoryCache cache)
        {
            _repository = repository;
            _emailService = emailService;
            _cache = cache;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> GetUserById(int id)
        {
           return await _repository.GetUserById(id);
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int roleId)
        {
           return await (_repository.UpdateUserRoleAsync(userId, roleId));
        }

        public async Task<User?> GetUserByUsername(string username)
      =>  await _repository.GetUserByUsername(username);


        public async Task<bool> UpdateUserProfileAsync(int personId, UserUpdateDto dto)
    => await _repository.UpdateUserProfileAsync(personId, dto);

        public async Task<UserInfoByCardDto?> GetUserInfoByCardIdAsync(int cardId)
      => await _repository.GetUserInfoByCardIdAsync(cardId);


        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            if (await _repository.EmailExistsAsync(dto.Email!))
                return (false, "Email đã tồn tại!");
            if (await _repository.UsernameExistsAsync(dto.Username))
                return (false, "Username đã tồn tại!");

            var user = new User
            {
                Name = dto.Name,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                Address = dto.Address,
                Email = dto.Email,
                Phone = dto.Phone,
                Username = dto.Username,
                Password = dto.Password, 
                RoleId = 1 
            };
            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();
            return (true, "Đăng ký thành công!");
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _repository.EmailExistsAsync(email);
        }

        public async Task<User> GetOrCreateGoogleUserAsync(Google.Apis.Auth.GoogleJsonWebSignature.Payload googlePayload)
        {
            var user = await _repository.GetByEmailAsync(googlePayload.Email);

            if (user == null)
            {
                user = new User
                {
                    Name = googlePayload.Name ?? googlePayload.Email,
                    Email = googlePayload.Email,
                    Username = googlePayload.Email, // hoặc sửa theo nghiệp vụ
                    RoleId = 1,      // Đăng nhập Google thì bạn đọc mặc định roleId = 1
                                     // Set thêm các trường khác nếu muốn (Avatar, Ngày sinh ...)
                };
                await _repository.CreateUserAsync(user);
            }
            return user;
        }

        public async Task SendForgotPasswordMailAsync(ForgotPasswordRequest req)
        {
            var user = await _repository.GetUserByEmailAsync(req.Email);
            if (user == null)
                throw new Exception("Email không tồn tại!");

            var otp = new Random().Next(100000, 999999).ToString();
            string cacheKey = $"otp_{user.Email!.ToLower()}";
            // Lưu OTP vào cache, có hạn 15 phút
            _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(15));

            string subject = "Mã xác nhận khôi phục mật khẩu";
            string body = $@"
        <div style='font-family:Roboto,sans-serif;background:#f6fff7;padding:18px 12px 8px 12px;border-radius:12px;'>
            <h3 style='color:#318b38;margin-bottom:12px'>Xác nhận quên mật khẩu</h3>
            <div>Tài khoản: <b>{user.Username}</b></div>
            <div>Mã xác nhận để đặt lại mật khẩu của bạn là:</div>
            <div style='font-size:2.2em;background:#e6ffd5;display:inline-block;border-radius:8px;padding:10px 22px;margin:18px 0;color:#369a01;font-weight:bold;letter-spacing:3px'>
                {otp}
            </div>
            <div style='color:#e65620;font-size:1.01em;margin:14px 0 6px 0'>Lưu ý: mã hợp lệ trong 15 phút. Nếu bạn không yêu cầu khôi phục, vui lòng bỏ qua email này.</div>
            <div style='color:#118b58;font-size:0.97em;margin-top:10px'>Trân trọng,<br/>Quản lý thư viện</div>
        </div>";

            await _emailService.SendAsync(user.Email, subject, body);
        }


        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            string cacheKey = $"otp_{email.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out string cachedOtp))
            {
                if (cachedOtp == otp)
                    return true;
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest req)
        {
            string cacheKey = $"otp_{req.Email.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out string cachedOtp))
            {
                if (cachedOtp == req.ResetCode)
                {
                    var user = await _repository.GetUserByEmailAsync(req.Email);
                    if (user == null) return false;

                    user.Password = req.NewPassword; // TODO: Hash password!
                    await _repository.UpdateUserAsync(user);
                    _cache.Remove(cacheKey); // OTP chỉ dùng 1 lần
                    return true;
                }
            }
            return false;
        }
    }
}
