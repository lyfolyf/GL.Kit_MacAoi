using System;
using System.Security.Cryptography;
using System.Text;

namespace GL.Kit.Security.Cryptography
{
    public static class MD5
    {
        internal static byte[] Encrypt(string input)
        {
            return Encrypt(Encoding.UTF8.GetBytes(input));
        }

        internal static byte[] Encrypt(byte[] inputBytes)
        {
            using (System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider())
            {
                return md5.ComputeHash(inputBytes);
            }
        }

        public static string EncryptBase64(string input)
        {
            byte[] m = Encrypt(input);
            return Convert.ToBase64String(m);
        }

        public static string Encrypt32(string input)
        {
            byte[] m = Encrypt(input);
            StringBuilder sb = new StringBuilder(32);
            foreach (byte b in m)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string EncryptBase64(byte[] input)
        {
            byte[] m = Encrypt(input);
            return Convert.ToBase64String(m);
        }

        public static string Encrypt32(byte[] input)
        {
            byte[] m = Encrypt(input);
            StringBuilder sb = new StringBuilder(32);
            foreach (byte b in m)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
