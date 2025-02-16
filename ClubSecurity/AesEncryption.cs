using System;
using System.Security.Cryptography;
using System.Text;

namespace CompClubAPI
{
    public class AesEncryption
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;// 32 байта для AES-256
        private readonly byte[] _iv; // 16 байт для IV

        public AesEncryption(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = HexStringToByteArray(_configuration["secretData:secretKey"]!); 
            _iv = HexStringToByteArray(_configuration["secretData:secretIV"]!);
        }
        public byte[] Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
        }

        public string Decrypt(byte[] cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}   