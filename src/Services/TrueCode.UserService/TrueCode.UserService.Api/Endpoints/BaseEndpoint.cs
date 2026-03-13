using Carter;
using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;

namespace TrueCode.UserService.Api.Endpoints;

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
    
    protected async Task<IResult> ExecuteCommandAsync<TCommand, TResult>(
        TCommand command,
        Func<TCommand, Task<CommandResult<TResult>>> commandHandler,
        ILogger logger
    )
    {
        try
        {
            var result = await commandHandler(command);

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
    
    protected async Task<IResult> ExecuteCommandAsync<TCommand>(
        TCommand command,
        Func<TCommand, Task<CommandResult>> commandHandler,
        ILogger logger
    )
    {
        try
        {
            var result = await commandHandler(command);

            if (result.IsSuccess)
                return Results.Ok();

            return ErrorResponse(result.Errors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Results.StatusCode(500);
        }
    }
}