using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.Core.Users;
using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class AddFavoriteCurrencyEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/currencies/favorites", HandlePost)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<IReadOnlyList<Error>>(StatusCodes.Status400BadRequest)
            .WithSummary("Добавить любимую валюту по текущему пользователю");
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