using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Queries;
using TrueCode.UserService.Application.Currencies.Queries;

namespace TrueCode.UserService.Api.Endpoints.Currencies;

public class GetFavoriteCurrenciesEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/currencies/favorites", HandleGet);
    }
    
    private async Task<IResult> HandleGet(HttpContext ctx,
        [FromServices] BaseQueryHandler<GetFavoriteCurrenciesQuery, List<string>> queryHandler,
        [FromServices] ILogger<GetFavoriteCurrenciesEndpoint> logger)
    {
        if (ctx.User.Identity?.IsAuthenticated == false)
            return Results.Unauthorized();
        
        var query = new GetFavoriteCurrenciesQuery();

        return await ExecuteQueryAsync(query,
            async qry => await queryHandler.ExecuteAsync(query, ctx.RequestAborted),
            logger);
    }
}