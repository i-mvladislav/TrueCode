using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrueCode.UserService.Api.Services;

namespace TrueCode.UserService.Api.Middlewares;

public sealed class JwtBlacklistMiddleware(RequestDelegate next, JwtBlacklistService blacklistService)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.User?.Identity?.IsAuthenticated == true)
        {
            var jti = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (!string.IsNullOrWhiteSpace(jti) && await blacklistService.HasToken(jti))
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await next(ctx);
    }
}