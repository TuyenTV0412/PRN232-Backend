using Microsoft.Extensions.Caching.Memory;
public class ResetPwService
{
    private readonly IMemoryCache _cache;
    public ResetPwService(IMemoryCache cache) { _cache = cache; }

    public void SaveOtp(string email, string code, TimeSpan timeout)
    {
        _cache.Set($"otp_{email.ToLower()}", code, timeout);
    }
    public string? GetOtp(string email)
    {
        _cache.TryGetValue($"otp_{email.ToLower()}", out string code);
        return code;
    }
    public void RemoveOtp(string email)
    {
        _cache.Remove($"otp_{email.ToLower()}");
    }
}
