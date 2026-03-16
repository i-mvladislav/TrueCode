using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrueCode.UserService.Api.Services;

namespace TrueCode.UserService.Api.Endpoints.Auth;

public class SignOutEndpoint(JwtBlacklistService blacklistService) : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/signOut", async (HttpContext ctx) =>
        {
            var jti = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var expires = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Exp);

            if (!string.IsNullOrWhiteSpace(jti) && long.TryParse(expires, out var expiresLong))
            {
                var ttl = TimeSpan.FromSeconds(expiresLong - DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                if (ttl.TotalSeconds > 0)
                {
                    await blacklistService.AddToken(jti, ttl);
                    return Results.Ok();
                }
            }

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Выйти из аккаунта");
    }
}