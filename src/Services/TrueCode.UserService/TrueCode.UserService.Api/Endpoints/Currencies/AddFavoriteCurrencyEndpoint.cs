using System.Security.Claims;
using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class AddFavoriteCurrencyEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/currencies/favorites", HandlePost);
    }

    private async Task<IResult> HandlePost(HttpContext ctx,
        AddFavoriteCurrencyRequest request,
        AddFavoriteCurrencyCommandHandler commandHandler)
    {
        if (ctx.User.Identity?.IsAuthenticated == false)
            return Results.Unauthorized();
        
        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new AddFavoriteCurrencyCommand
        {
            UserId = new Guid(userId),
            CurrencyCode = request.CurrencyCode,
        };

        var result = await commandHandler.ExecuteAsync(command, ctx.RequestAborted);

        if (!result.IsSuccess)
        {
            return Results.Json(new
            {
                Errors = result.Errors
            });
        }
        
        return Results.Ok();
    }
}