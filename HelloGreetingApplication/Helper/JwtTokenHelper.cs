using Microsoft.IdentityModel.Tokens;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelloGreeting.Helper
{
    public class JwtTokenHelper
    {
        private readonly IConfiguration _configuration;
        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object is null.");
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            if (jwtSettings == null)
            {
                throw new Exception("JWT settings are missing in configuration.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userName", user.FirstName),
                new Claim("email", user.Email)
            };

            var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials
        );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        internal string GenerateToken(bool user)
        {
            throw new NotImplementedException();
        }

        internal object GenerateToken(LoginModel user)
        {
            throw new NotImplementedException();
        }
    }
}