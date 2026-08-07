using System.Security.Cryptography;
using System.Text;

namespace ProjectCore.Util
{
    /// <summary>
    /// MD5工具类
    /// </summary>
    public static class MD5Helper
    {
        /// <summary>
        /// MD5加密，返回32位小写字符串
        /// </summary>
        /// <param name="input">原文</param>
        /// <returns>MD5值</returns>
        public static string Encrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        /// <summary>
        /// MD5校验
        /// </summary>
        /// <param name="input">原文</param>
        /// <param name="md5Value">MD5值</param>
        /// <returns>是否一致</returns>
        public static bool Verify(string input, string md5Value)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(md5Value))
            {
                return false;
            }

            return Encrypt(input) == md5Value.Trim().ToLowerInvariant();
        }
    }
}
