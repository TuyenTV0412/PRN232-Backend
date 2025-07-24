using Backend.DTO;
using Backend.Model;
using Backend.Service.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleDto dto)
        {
            var result = await _userService.UpdateUserRoleAsync(id, dto.RoleId);
            if (!result) return NotFound();
            return NoContent();
        }

        // GET: api/User/Username/{username}
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateUserProfileAsync(id, dto);
            if (!result) return NotFound(new { message = "Không tìm thấy user." });
            return NoContent();
        }

        [HttpGet("check/{cardId}")]
        public async Task<IActionResult> CheckCard(int cardId)
        {
            var userInfo = await _userService.GetUserInfoByCardIdAsync(cardId);
            if (userInfo == null)
                return NotFound(new { message = "Thẻ không tồn tại hoặc không gắn với user nào!" });
            return Ok(userInfo);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { exists = false, message = "Email không hợp lệ!" });

            bool exists = await _userService.EmailExistsAsync(email);
            return Ok(new { exists });
        }


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                // Validate token Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["Google:ClientId"] } // Lấy từ appsettings.json
                });
                if (!payload.EmailVerified)
                    return BadRequest(new { message = "Email Google chưa được xác thực!" });

                // Tìm hoặc tạo user trong DB
                var user = await _userService.GetOrCreateGoogleUserAsync(payload);

                // Tạo JWT token hệ thống
                var token = JwtHelper.GenerateJwt(user, _config);

                // Trả về thông tin cho frontend
                return Ok(new
                {
                    token,
                    username = user.Username,
                    roleId = user.RoleId,
                    personId = user.PersonId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Google xác thực thất bại: " + ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            try
            {
                await _userService.SendForgotPasswordMailAsync(req);
                return Ok(new { message = "Đã gửi mã xác nhận về email! Vui lòng kiểm tra." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest req)
        {
            bool ok = await _userService.VerifyOtpAsync(req.Email, req.ResetCode);
            if (ok)
                return Ok(new { message = "Mã xác thực hợp lệ. Bạn có thể đặt lại mật khẩu mới!" });
            return BadRequest(new { message = "Mã xác thực không đúng hoặc đã hết hạn!" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            bool ok = await _userService.ResetPasswordAsync(req);
            if (ok)
                return Ok(new { message = "Đổi mật khẩu thành công! Bạn có thể đăng nhập bằng mật khẩu mới." });
            return BadRequest(new { message = "OTP không đúng hoặc đã hết hạn!" });
        }
    }


    public class UpdateRoleDto
    {
        public int RoleId { get; set; }
    }
}
