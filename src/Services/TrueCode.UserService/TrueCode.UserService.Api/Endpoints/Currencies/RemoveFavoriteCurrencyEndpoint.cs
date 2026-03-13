using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class RemoveFavoriteCurrencyEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/currencies/favorites", HandleDelete);
    }
    
    private async Task<IResult> HandleDelete(HttpContext ctx,
        [FromBody] RemoveFavoriteCurrencyRequest request,
        [FromServices] RemoveFavoriteCurrencyCommandHandler commandHandler)
    {
        if (ctx.User.Identity?.IsAuthenticated == false)
            return Results.Unauthorized();
        
        var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new RemoveFavoriteCurrencyCommand
        {
            UserId = new Guid(userId),
            Name = request.Name,
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