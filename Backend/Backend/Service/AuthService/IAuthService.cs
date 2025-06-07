﻿namespace Backend.Service.AuthService
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}
