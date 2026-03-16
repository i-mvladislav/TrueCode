using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.Core.Users;
using TrueCode.UserService.Application.Currencies.Queries;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class GetFavoriteCurrenciesEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/currencies/favorites", HandleGet)
            .Produces<List<string>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<IReadOnlyList<Error>>(StatusCodes.Status400BadRequest)
            .WithSummary("Получить список валют по текущему пользователю");
    }
    
    private async Task<IResult> HandleGet(HttpContext ctx,
        [FromServices] BaseQueryHandler<GetFavoriteCurrenciesQuery, List<string>> queryHandler,
        [FromServices] ICurrentUserContext userContext,
        [FromServices] ILogger<GetFavoriteCurrenciesEndpoint> logger)
    {
        if (!userContext.IsAuthenticated)
            return Results.Unauthorized();
        
        var query = new GetFavoriteCurrenciesQuery();

        return await ExecuteQueryAsync(query,
            async qry => await queryHandler.ExecuteAsync(query, ctx.RequestAborted),
            logger);
    }
}