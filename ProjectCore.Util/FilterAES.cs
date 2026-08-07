using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Util
{
    /// <summary>
    /// 慢病系统
    /// </summary>
    public class FilterAES
    {

        /// <summary>
        /// 加密秘钥
        /// </summary>
        public static string key = "GVvVJyrsFRKms8XKhwfwpgB47DtIaZ2p";

        /// <summary>
        /// 加密偏移量
        /// </summary>
        public static string iv = "4XEUxWxkTSGcEZxe";

        /// <summary>
        /// 加密编码方式
        /// </summary>
        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        ///  加密 参数：string
        /// </summary>
        /// <param name="strCon">加密内容</param>
        /// <returns>string：密文</returns>
        public static string FileterEncrypt(string strCon)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strCon))
                {
                    return null;
                }

                byte[] byCon = encoding.GetBytes(strCon);
                var rm = new RijndaelManaged
                {
                    IV = encoding.GetBytes(iv),
                    Key = encoding.GetBytes(key),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rm.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(byCon, 0, byCon.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解密 参数：string
        /// </summary>
        /// <param name="strCon">解密内容</param>
        /// <returns></returns>
        public static string FileterDecrypt(string strCon)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strCon))
                {
                    return null;
                }

                byte[] byCon = Convert.FromBase64String(strCon);
                var rm = new RijndaelManaged
                {
                    IV = encoding.GetBytes(iv),
                    Key = encoding.GetBytes(key),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rm.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(byCon, 0, byCon.Length);
                return encoding.GetString(resultArray);
            }
            catch
            {
                return "";
            }
        }
    }
}
