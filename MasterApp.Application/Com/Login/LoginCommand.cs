using MasterApp.Application.Interface;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Threading;
using Dapper;
using MasterApp.Application.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using static MasterApp.Application.Common.Exceptions.ValidationException;

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
        var token = _tokenService.GenerateJWToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new LoginResultDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            UserName = user.UserName
        };
    }
}
