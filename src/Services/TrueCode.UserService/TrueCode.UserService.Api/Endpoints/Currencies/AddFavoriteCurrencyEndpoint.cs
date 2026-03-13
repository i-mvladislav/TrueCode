using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Commands;
using TrueCode.Core.Users;
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
        [FromBody] AddFavoriteCurrencyRequest request,
        [FromServices] BaseCommandHandler<AddFavoriteCurrencyCommand> commandHandler,
        [FromServices] ICurrentUserContext userContext,
        [FromServices] ILogger<AddFavoriteCurrencyEndpoint> logger)
    {
        if (!userContext.IsAuthenticated)
            return Results.Unauthorized();
        
        var command = new AddFavoriteCurrencyCommand
        {
            Name = request.Name,
        };

        return await ExecuteCommandAsync(
            command,
            async cmd => await commandHandler.ExecuteAsync(cmd, ctx.RequestAborted),
            logger
        );
    }
}