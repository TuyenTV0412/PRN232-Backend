using Backend.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend
{
    public static class JwtHelper
    {
        public static string GenerateJwt(User user, IConfiguration config)
        {
            var claims = new List<Claim>
        {
            new Claim("userId", user.PersonId.ToString()),
            new Claim("username", user.Username),
            new Claim("roleId", user.RoleId.ToString())
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
