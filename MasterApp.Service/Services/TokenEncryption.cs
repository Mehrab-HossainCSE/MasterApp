using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MasterApp.Service.Services;

public class TokenEncryption : ITokenEncryption
{
    private readonly string _key;

    public TokenEncryption(IOptions<ApiSettingsEncryption> options)
    {
        var key = options.Value.Key;  // read from appsettings.json via IOptions

        if (string.IsNullOrWhiteSpace(key) || (key.Length != 16 && key.Length != 24 && key.Length != 32))
            throw new ArgumentException("Key must be 16, 24, or 32 characters long.");

        _key = key;
    }

    public string TokenEncrypt(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("Plain text cannot be null or empty.");

        // Split the plainText by ~ to get username and password
        var parts = plainText.Split('~');
        if (parts.Length != 2)
            throw new ArgumentException("Plain text must contain username and password separated by '~'.");

        string username = parts[0];
        string password = parts[1];

        // Encrypt username and password separately
        string encryptedUsername = EncryptString(username);
        string encryptedPassword = EncryptString(password);

        // Combine with ~ separator
        return encryptedUsername + "~" + encryptedPassword;
    }

    public string TokenDecrypt(string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
            throw new ArgumentException("Cipher text cannot be null or empty.");

        // Split the cipherText by ~ to get encrypted username and password
        var parts = cipherText.Split('~');
        if (parts.Length != 2)
            throw new ArgumentException("Cipher text must contain encrypted username and password separated by '~'.");

        string encryptedUsername = parts[0];
        string encryptedPassword = parts[1];

        // Decrypt username and password separately
        string username = DecryptString(encryptedUsername);
        string password = DecryptString(encryptedPassword);

        // Combine with ~ separator
        return username + "~" + password;
    }

    private string EncryptString(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.GenerateIV();

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string DecryptString(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(cipher))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}