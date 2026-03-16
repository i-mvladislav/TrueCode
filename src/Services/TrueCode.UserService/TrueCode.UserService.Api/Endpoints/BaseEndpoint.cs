using Carter;
using TrueCode.Core.Commands;
using TrueCode.Core.Enums;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;

namespace TrueCode.UserService.Api.Endpoints;

public abstract class BaseEndpoint : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);
    
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
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.Unauthorized))
                return Results.Unauthorized();
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.Validation))
                return Results.BadRequest(result.Errors.Where(e => e.ErrorType == ErrorType.Validation));
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.NotFound))
                return Results.NotFound(result.Errors.Where(e => e.ErrorType == ErrorType.NotFound));

            throw new NotImplementedException();
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

            if (result.Errors.Any(e => e.ErrorType == ErrorType.Unauthorized))
                return Results.Unauthorized();
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.Validation))
                return Results.BadRequest(result.Errors.Where(e => e.ErrorType == ErrorType.Validation));
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.NotFound))
                return Results.NotFound(result.Errors.Where(e => e.ErrorType == ErrorType.NotFound));

            throw new NotImplementedException();
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

            if (result.Errors.Any(e => e.ErrorType == ErrorType.Unauthorized))
                return Results.Unauthorized();
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.Validation))
                return Results.BadRequest(result.Errors.Where(e => e.ErrorType == ErrorType.Validation));
            
            if (result.Errors.Any(e => e.ErrorType == ErrorType.NotFound))
                return Results.NotFound(result.Errors.Where(e => e.ErrorType == ErrorType.NotFound));

            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Results.StatusCode(500);
        }
    }
}