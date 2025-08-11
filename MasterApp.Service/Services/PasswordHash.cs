using MasterApp.Application.Interface;
using System.Security.Cryptography;
using System.Text;

namespace MasterApp.Service.Services;

public class PasswordHash : IPasswordHash
{
    public const int SALT_BYTE_SIZE = 24;
    public const int HASH_BYTE_SIZE = 24;
    public const int PBKDF2_ITERATIONS = 1000;
    public const int ITERATION_INDEX = 0;
    public const int SALT_INDEX = 1;
    public const int PBKDF2_INDEX = 2;

    public string CreateHash(string password, ref string passwordHash, ref string passwordSalt)
    {
        if (passwordHash == null) throw new ArgumentNullException("passwordHash");
        if (passwordSalt == null) throw new ArgumentNullException("passwordSalt");
        // Generate a random salt
        RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
        byte[] salt = new byte[SALT_BYTE_SIZE];
        csprng.GetBytes(salt);

        // Hash the password and encode the parameters
        byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

        passwordHash = Convert.ToBase64String(hash);
        passwordSalt = Convert.ToBase64String(salt);

        return PBKDF2_ITERATIONS + ":" +
            Convert.ToBase64String(salt) + ":" +
            Convert.ToBase64String(hash);
    }
    public bool ValidatePassword(string password, string passwordHash, string passwordSalt)
    {

        char[] delimiter = { ':' };
        int iterations = PBKDF2_ITERATIONS;
        byte[] salt = Convert.FromBase64String(passwordSalt);
        byte[] hash = Convert.FromBase64String(passwordHash);

        byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
        return SlowEquals(hash, testHash);
    }
    private bool SlowEquals(byte[] a, byte[] b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }
    private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
    {
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
        pbkdf2.IterationCount = iterations;
        return pbkdf2.GetBytes(outputBytes);
    }

    public string GetEncryptedPassword(string password)
    {
        string pass = "";
        byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(password);
        string returnValue = Convert.ToBase64String(toEncodeAsBytes);
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 objMd5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(returnValue);
        encodedBytes = objMd5.ComputeHash(originalBytes);
        StringBuilder ss = new StringBuilder();
        foreach (byte b in encodedBytes)
        {
            ss.Append(b.ToString("x2".ToLower()));
        }
        pass = ss.ToString();
        return pass;
    }
}
