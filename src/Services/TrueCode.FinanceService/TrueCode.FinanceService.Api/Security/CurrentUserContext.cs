using System.Security.Claims;
using TrueCode.Core.Users;

namespace TrueCode.FinanceService.Api.Security;

public class CurrentUserContext(IHttpContextAccessor context) : ICurrentUserContext
{
    public bool IsAuthenticated =>
        context.HttpContext?.User?.Identity?.IsAuthenticated == true && Authorization is not null && UserId is not null;
    
    public string? UserId =>
        context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Authorization =>
        context.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
}