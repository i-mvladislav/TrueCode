using System.Security.Claims;
using TrueCode.UserService.Application.Currencies.Queries;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class GetFavoriteCurrenciesEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/currencies/favorites", HandleGet);
    }
    
    private async Task<IResult> HandleGet(HttpContext ctx,
        GetFavoriteCurrenciesQueryHandler queryHandler)
    {
        if (ctx.User.Identity?.IsAuthenticated == false)
            return Results.Unauthorized();
        
        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new GetFavoriteCurrenciesQuery
        {
            UserId = new Guid(userId),
        };

        var result = await queryHandler.ExecuteAsync(command, ctx.RequestAborted);

        if (!result.IsSuccess)
        {
            return Results.Json(new
            {
                Errors = result.Errors
            });
        }
        
        return Results.Json(result.Data);
    }
}