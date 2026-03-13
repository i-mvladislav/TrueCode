using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Commands;
using TrueCode.Core.Users;
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
        [FromServices] BaseCommandHandler<RemoveFavoriteCurrencyCommand> commandHandler,
        [FromServices] ICurrentUserContext userContext,
        [FromServices] ILogger<RemoveFavoriteCurrencyEndpoint> logger)
    {
        if (!userContext.IsAuthenticated)
            return Results.Unauthorized();

        var command = new RemoveFavoriteCurrencyCommand
        {
            Name = request.Name,
        };

        return await ExecuteCommandAsync(
            command,
            async cmd => await commandHandler.ExecuteAsync(command, ctx.RequestAborted),
            logger
        );
    }
}