namespace MasterApp.Application.Interface;

public interface ITokenEncryption
{
    string TokenEncrypt(string plainText);
    string TokenDecrypt(string cipherText);
}
