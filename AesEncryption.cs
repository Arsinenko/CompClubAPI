using System.Security.Cryptography;
using System.Text;

namespace CompClubAPI;

public class AesEncryption
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4"); //TODO change key to something more secure
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4"); //TODO change iv to something more secure

    public static string Encrypt(string plainText)
    {
        byte[] encrypted = Aes.Create().CreateEncryptor(key, iv).TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, Encoding.UTF8.GetByteCount(plainText));
        return Convert.ToBase64String(encrypted);
    }
    public static string Decrypt(string cipherText)
    {
        byte[] decrypted = Aes.Create().CreateDecryptor(key, iv).TransformFinalBlock(Convert.FromBase64String(cipherText), 0, Convert.FromBase64String(cipherText).Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}