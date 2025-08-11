using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Service.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MasterApp.Service.Services;

public class TokenService : ITokenService
{
    private readonly JWTSettings _jwtSettings;
    public TokenService(IOptions<JWTSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    //public JwtSecurityToken GenerateJWToken(User? user,  Employee? employee, Branch? branch)
    //{

    //    var claims = new List<Claim>
    //    {
    //        new Claim(ClaimTypes.UserData,JsonSerializer.Serialize(user)),
    //        new Claim(ClaimTypes.Name, user.UserName ?? ""),
    //        new Claim("uid", user.UserID?? ""),
    //        new Claim("branchName", branch.BranchName ?? ""),
    //        new Claim("branchId",branch.BranchID ?? ""),
    //        new Claim("employeeId",employee.EmpID ?? ""),
    //        new Claim("employeeName",employee.EmpName ?? "")

    //    };
    //    return JWTGeneration(claims);
    //}
    public JwtSecurityToken GenerateJWToken(UserDto user)
    {


        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(user)),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim("uid", user.UserID ?? ""),
        //new Claim("branchName", branch.BranchName ?? ""),
        //new Claim("branchId", branch.BranchID ?? ""),
        //new Claim("employeeId", employee.EmpID ?? ""),
        //new Claim("employeeName", employee.EmpName ?? ""),
        //new Claim("companyName", companyInfo.Name ?? ""),
        //new Claim("IsBranchIdAuto", companyInfo.IsBranchIdAuto ? "true" : "false"),
    };

        return JWTGeneration(claims);
    }

    private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims ?? Enumerable.Empty<Claim>(),
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenValidityInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}

