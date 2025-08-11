using System.Globalization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MasterApp.Application.Common.Models;

namespace MasterApp.Application.Interface;

public interface ITokenService
{
    JwtSecurityToken GenerateJWToken(UserDto user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

