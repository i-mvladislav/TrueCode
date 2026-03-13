using Carter;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;

namespace TrueCode.FinanceService.Api.Endpoints;

public abstract class BaseEndpoint : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);
    
    protected static IResult ErrorResponse(IEnumerable<Error> errors)
    {
        return Results.Json(new { Errors = errors });
    }

    protected async Task<IResult> ExecuteQueryAsync<TQuery, TResult>(
        TQuery query,
        Func<TQuery, Task<QueryResult<TResult>>> queryHandler,
        ILogger logger
    )
    {
        try
        {
            var result = await queryHandler(query);

            if (result.IsSuccess)
                return Results.Json(result.Data);

            return ErrorResponse(result.Errors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Results.StatusCode(500);
        }
    }
}