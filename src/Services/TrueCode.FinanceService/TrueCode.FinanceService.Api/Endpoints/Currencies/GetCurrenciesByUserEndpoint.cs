using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Queries;
using TrueCode.Core.Users;
using TrueCode.FinanceService.Application.Currencies.Models;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;

namespace TrueCode.FinanceService.Api.Endpoints.Currencies;

public class GetCurrenciesByUserEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/currencies", HandleGet);
    }
    
    private async Task<IResult> HandleGet(
        [FromServices] BaseQueryHandler<GetCurrenciesByUserQuery, List<Currency>> queryHandler, 
        [FromServices] ILogger<GetCurrenciesByUserEndpoint> logger,
        [FromServices] ICurrentUserContext userContext,
        HttpContext ctx)
    {
        if (!userContext.IsAuthenticated)
            return Results.Unauthorized();
        
        var query = new GetCurrenciesByUserQuery();
        return await ExecuteQueryAsync(
            query,
            async qry => await queryHandler.ExecuteAsync(qry, ctx.RequestAborted),
            logger);
    }
}