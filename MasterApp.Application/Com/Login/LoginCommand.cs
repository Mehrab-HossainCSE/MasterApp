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

    public LoginCommand(IDbConnectionFactory context, ITokenService tokenService, IPasswordHash passwordHash)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHash = passwordHash;
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
                Email = "systemuser@gmail.com"
            };

            // Generate token for the fake user
            var token = _tokenService.GenerateJWToken(fakeUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new LoginResultDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserName = fakeUser.UserName
            };
        }

        // ---- Normal DB login process ----
        using var connection = _context.CreateConnection("MasterAppDB");

        const string sql = @"SELECT TOP 1 * FROM Users WHERE Email = @Email";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(
            sql,
            new { Email = request.UserName }
        );

        if (user == null)
            throw new LoginFailedException();

        var isLoginValid = _passwordHash.ValidatePassword(request.Password, user.PasswordHash, user.PasswordSalt);

        if (!isLoginValid)
            throw new LoginFailedException();

        // Generate JWT token
        var dbToken = _tokenService.GenerateJWToken(user);
        var dbRefreshToken = _tokenService.GenerateRefreshToken();

        return new LoginResultDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(dbToken),
            RefreshToken = dbRefreshToken,
            UserName = user.UserName,
            UserID = user.UserID,
        };
    }

}
