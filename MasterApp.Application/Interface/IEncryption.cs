namespace MasterApp.Application.Interface;

public interface IEncryption
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
