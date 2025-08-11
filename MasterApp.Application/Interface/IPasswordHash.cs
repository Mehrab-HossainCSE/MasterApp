namespace MasterApp.Application.Interface;

public interface IPasswordHash
{
    string CreateHash(string password, ref string passwordHash, ref string passwordSalt);
    bool ValidatePassword(string password, string passwordHash, string passwordSalt);
    string GetEncryptedPassword(string password);
}
