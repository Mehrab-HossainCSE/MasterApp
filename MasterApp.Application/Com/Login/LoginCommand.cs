using MasterApp.Application.Interface;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Threading;
using Dapper;
using MasterApp.Application.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using static MasterApp.Application.Common.Exceptions.ValidationException;
using System.Reflection;

namespace MasterApp.Application.Com.Login;

public class LoginCommand
{
    private readonly IDbConnectionFactory _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHash _passwordHash;
    private readonly ITokenEncryption _encrytionToken;
    
 
    public LoginCommand(IDbConnectionFactory context, ITokenService tokenService, IPasswordHash passwordHash, ITokenEncryption encrytionToken)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHash = passwordHash;
        _encrytionToken = encrytionToken;
       
    }

    public async Task<LoginResultDto> ExecuteAsync(LoginDto request, CancellationToken cancellationToken)
    {
        // Special hardcoded login (no DB check)
        if (request.UserName == "systemuser@gmail.com" && request.Password == "...")
        {
            // Create a fake user object
            var fakeUser = new UserDto
            {
                UserName = "System User",
                Email = "systemuser@gmail.com",
               
            };

            // Generate token for the fake user
            var token = _tokenService.GenerateJWToken(fakeUser);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var encriptedPass = _encrytionToken.TokenEncrypt(request.UserName + "~" + request.Password);
            var de = _encrytionToken.TokenDecrypt(encriptedPass);

            return new LoginResultDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserName = fakeUser.Email,
                UserID = "0",
                token = encriptedPass,
            };
        }

        // ---- Normal DB login process ----
        using var connection = _context.CreateConnection("MasterAppDB");

        const string sql = @"SELECT TOP 1 * FROM Users WHERE UserName = @UserName";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(
            sql,
            new { UserName = request.UserName }
        );

        if (user == null)
            throw new LoginFailedException();

        var isLoginValid = _passwordHash.ValidatePassword(request.Password, user.PasswordHash, user.PasswordSalt);

        if (!isLoginValid)
            throw new LoginFailedException();

        // Generate JWT token
        var dbToken = _tokenService.GenerateJWToken(user);
        var dbRefreshToken = _tokenService.GenerateRefreshToken();
        var dbencriptedPass = _encrytionToken.TokenEncrypt(user.UserName + "~" + request.Password);
        var decryptedPass = _encrytionToken.TokenDecrypt(dbencriptedPass);
        return new LoginResultDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(dbToken),
            RefreshToken = dbRefreshToken,
            UserName = user.UserName,
            UserID = user.UserID,
            token = dbencriptedPass
        };
    }

}
