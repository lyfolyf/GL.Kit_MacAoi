using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GL.Kit.Security.Cryptography
{
    public static class DES
    {
        internal static byte[] _Encrypt(string input, string key)
        {
            key = MD5.EncryptBase64(key).Substring(0, 8);

            byte[] result;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                IV = Encoding.UTF8.GetBytes(key)
            };

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(input);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                result = ms.ToArray();
            }

            des.Clear();

            return result;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input">需要加密的内容</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string input, string key)
        {
            if (input == string.Empty) return string.Empty;

            byte[] bytes = _Encrypt(input, key);
            StringBuilder result = new StringBuilder();
            foreach (byte b in bytes)
            {
                result.AppendFormat("{0:X2}", b);
            }

            return result.ToString();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">需要解密的内容</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Decrypt(string input, string key)
        {
            if (input == string.Empty) return string.Empty;

            key = MD5.EncryptBase64(key).Substring(0, 8);

            string result;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = new byte[input.Length / 2];
                    for (int i = 0, len = input.Length / 2; i < len; i++)
                    {
                        bytes[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
                    }

                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                result = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
            }

            des.Clear();

            return result;
        }
    }
}
