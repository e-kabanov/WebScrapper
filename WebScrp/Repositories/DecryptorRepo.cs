using System.Security.Cryptography;
using System.Text;
using WebScrp.DTOs;

namespace WebScrp.Repositores;

public interface IDecryptorRepo
{
    Task<string> Decrypt(InputDataDto inputData);
}

public class DecryptorRepo : IDecryptorRepo
{
    public async Task<string> Decrypt(InputDataDto inputData)
    {
        byte[] cipherBytes = Convert.FromBase64String(inputData.EncryptedTextBytesBase64);

        if (cipherBytes.Length == 0 || cipherBytes.Length % 16 != 0)
        {
            throw new ArgumentException("Длина должна быть кратна 16 байтам!");
        }

        byte[] keyBytes = Convert.FromBase64String(inputData.KeyBytesBase64);

        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = keyBytes;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;

        using var decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}