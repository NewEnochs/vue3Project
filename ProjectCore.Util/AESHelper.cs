using System.Security.Cryptography;
using System.Text;

namespace ProjectCore.Util
{
    /// <summary>
    /// AES可逆加解密工具类
    /// </summary>
    public static class AESHelper
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        public static string Key { get; set; } = "GVvVJyrsFRKms8XKhwfwpgB47DtIaZ2p";

        /// <summary>
        /// 初始化向量
        /// </summary>
        public static string Iv { get; set; } = "4XEUxWxkTSGcEZxe";

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText))
                {
                    return string.Empty;
                }

                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                using ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] resultBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cipherText))
                {
                    return string.Empty;
                }

                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] inputBytes = Convert.FromBase64String(cipherText);
                using ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] resultBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Encoding.UTF8.GetString(resultBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
