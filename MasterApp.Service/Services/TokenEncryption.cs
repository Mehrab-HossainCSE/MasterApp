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

        // Combine username and password with ~ separator, then encrypt the whole string
        string combinedText = parts[0] + "~" + parts[1];
        return EncryptString(combinedText);
    }

    public string TokenDecrypt(string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
            throw new ArgumentException("Cipher text cannot be null or empty.");

        // Decrypt the whole string, which should contain username~password
        return DecryptString(cipherText);
    }

    private string EncryptString(string plainText)
    {
        using (var aes = new AesCryptoServiceProvider()) // .NET 4.5 compatible
        {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.GenerateIV();

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                // Write IV to the beginning of the stream
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                // Convert to URL-safe Base64
                return ToUrlSafeBase64(ms.ToArray());
            }
        }
    }

    private string DecryptString(string cipherText)
    {
        // Convert from URL-safe Base64 back to regular Base64
        var fullCipher = FromUrlSafeBase64(cipherText);

        using (var aes = new AesCryptoServiceProvider()) // .NET 4.5 compatible
        {
            aes.Key = Encoding.UTF8.GetBytes(_key);

            // Extract IV from the beginning of the cipher
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

    // Convert regular Base64 to URL-safe Base64
    private string ToUrlSafeBase64(byte[] data)
    {
        return Convert.ToBase64String(data)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", ""); // Remove padding
    }

    // Convert URL-safe Base64 back to regular Base64
    private byte[] FromUrlSafeBase64(string urlSafeBase64)
    {
        string base64 = urlSafeBase64
            .Replace('-', '+')
            .Replace('_', '/');

        // Add padding if necessary
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}