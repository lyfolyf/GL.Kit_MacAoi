using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GL.Kit.Security.Cryptography
{
    // .NET Framework 中提供的 RSA 算法规定：
    // 待加密的字节数不能超过密钥的长度值除以 8 再减去 11（即：RSACryptoServiceProvider.KeySize / 8 - 11）

    public static class RSA
    {
        /// <summary>
        /// 创建公钥和私钥对
        /// </summary>
        /// <returns>(公钥,私钥)</returns>
        public static (string publicKey, string privateKey) CreateXmlKey()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);
                return (publicKey, privateKey);
            }
        }

        /// <summary>
        /// RAS加密
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="plaintext">明文</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(string xmlPublicKey, string plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                throw new ArgumentNullException(nameof(plaintext));

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                int MaxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

                byte[] plaintextBytes = new UnicodeEncoding().GetBytes(plaintext);

                if (plaintextBytes.Length <= MaxBlockSize)
                {
                    return rsa.Encrypt(plaintextBytes, false);
                }
                else
                {
                    using (MemoryStream plaiStream = new MemoryStream(plaintextBytes))
                    using (MemoryStream crypStream = new MemoryStream())
                    {
                        byte[] buffer = new byte[MaxBlockSize];

                        int blockSize;
                        while ((blockSize = plaiStream.Read(buffer, 0, MaxBlockSize)) > 0)
                        {
                            byte[] ciphertext;
                            if (blockSize != MaxBlockSize)
                            {
                                byte[] buffer1 = new byte[blockSize];
                                Array.Copy(buffer, 0, buffer1, 0, blockSize);

                                ciphertext = rsa.Encrypt(buffer1, false);
                            }
                            else
                            {
                                ciphertext = rsa.Encrypt(buffer, false);
                            }
                            crypStream.Write(ciphertext, 0, ciphertext.Length);
                        }
                        return crypStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// RAS解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="ciphertextBytes">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string xmlPrivateKey, byte[] ciphertextBytes)
        {
            if (ciphertextBytes == null || ciphertextBytes.Length == 0)
                throw new ArgumentNullException(nameof(ciphertextBytes));

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                int MaxBlockSize = rsa.KeySize / 8;

                if (ciphertextBytes.Length <= MaxBlockSize)
                {
                    return new UnicodeEncoding().GetString(rsa.Decrypt(ciphertextBytes, false));
                }
                else
                {
                    using (MemoryStream crypStream = new MemoryStream(ciphertextBytes))
                    using (MemoryStream plaiStream = new MemoryStream())
                    {
                        byte[] buffer = new byte[MaxBlockSize];

                        while (crypStream.Read(buffer, 0, MaxBlockSize) > 0)
                        {
                            byte[] plaintext = rsa.Decrypt(buffer, false);
                            plaiStream.Write(plaintext, 0, plaintext.Length);
                        }

                        return new UnicodeEncoding().GetString(plaiStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// RAS加密
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="plaintext">明文</param>
        /// <returns>密文</returns>
        public static string EncryptBase64(string xmlPublicKey, string plaintext)
        {
            byte[] buffer = Encrypt(xmlPublicKey, plaintext);

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// RAS解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="ciphertext">密文</param>
        /// <returns>明文</returns>
        public static string DecryptBase64(string xmlPrivateKey, string ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                throw new ArgumentNullException(nameof(ciphertext));

            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            return Decrypt(xmlPrivateKey, ciphertextBytes);
        }
    }
}
