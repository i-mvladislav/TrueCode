using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Api.Endpoints.Currencies;

public class GetCurrenciesByUserEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/currencies", HandleGet);
    }
    
    private async Task<IResult> HandleGet([FromServices] GetCurrenciesByUserQueryHandler queryHandler, HttpContext ctx)
    {
        var isAuthenticated = ctx.User.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated || string.IsNullOrWhiteSpace(ctx.Request.Headers.Authorization))
            return Results.Unauthorized();

        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var authorization = ctx.Request.Headers.Authorization!.First()!.Substring("Bearer ".Length);

        var query = new GetCurrenciesByUserQuery
        {
            UserId = new Guid(userId),
            JwtToken = authorization,
        };
        
        var result = await queryHandler.ExecuteAsync(query, ctx.RequestAborted);
        
        if (!result.IsSuccess)
        {
            return ErrorResponse(result.Errors);
        }
        
        return Results.Json(result.Data);
    }
}