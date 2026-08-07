using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using ProjectCore.DAL;

namespace ProjectCore.Web.Services
{
    public class AuthTokenCache
    {
        public const string MagicPassword = "Enochs@?2023";
        private const string Issuer = "ProjectCore";
        private const string Audience = "ProjectCore.Web";
        private const string SigningKey = "ProjectCore.Web.Jwt.SigningKey.2026.Enochs";
        private readonly IMemoryCache _cache;
        private readonly byte[] _keyBytes = Encoding.UTF8.GetBytes(SigningKey);

        public AuthTokenCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string CreateToken(Student student)
        {
            var expires = DateTime.UtcNow.AddHours(8);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, student.GUID ?? string.Empty),
                new(ClaimTypes.NameIdentifier, student.GUID ?? string.Empty),
                new(ClaimTypes.Name, student.LoginName),
                new("StudentName", student.StudentName),
                new("StudentCode", student.StudentCode)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(_keyBytes),
                SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            _cache.Set(GetCacheKey(token), new LoginUserInfo(student, expires), expires);

            return token;
        }

        public bool TryGetLoginUser(string token, out LoginUserInfo loginUser)
        {
            loginUser = null;

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch
            {
                return false;
            }

            return _cache.TryGetValue(GetCacheKey(token), out loginUser);
        }

        public void RemoveToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            _cache.Remove(GetCacheKey(token));
        }

        private static string GetCacheKey(string token)
        {
            return $"login:token:{token}";
        }
    }

    public class LoginUserInfo
    {
        public LoginUserInfo(Student student, DateTime expires)
        {
            StudentGUID = student.GUID ?? string.Empty;
            StudentCode = student.StudentCode;
            StudentName = student.StudentName;
            LoginName = student.LoginName;
            Expires = expires;
        }

        public string StudentGUID { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string LoginName { get; set; }
        public DateTime Expires { get; set; }
    }
}
