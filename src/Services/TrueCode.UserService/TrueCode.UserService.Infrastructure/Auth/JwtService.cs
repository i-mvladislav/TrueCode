using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TrueCode.UserService.Application.Auth.Models;
using TrueCode.UserService.Application.Contracts;
using TrueCode.UserService.Domain.Entities;
using TrueCode.UserService.Infrastructure.Configuration;

namespace TrueCode.UserService.Infrastructure.Auth;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public JwtToken GenerateToken(UserEntity user)
    {
        var settings = configuration.GetSection("Jwt").Get<JwtSettings>()!;
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

        var expires = DateTimeOffset.UtcNow.AddMinutes(10);
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Exp, expires.ToUnixTimeSeconds().ToString())
        ];

        var audiences = settings.Audiences.Select(a => new Claim(JwtRegisteredClaimNames.Aud, a)).ToList();
        claims.AddRange(audiences);
        
        var accessToken = new JwtSecurityToken(
            issuer: settings.Issuer,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtToken
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            Expires = accessToken.ValidTo,
        };
    }
}