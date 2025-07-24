using Backend.DTO;

namespace Backend.Service.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(string username, string password);
    }
}
