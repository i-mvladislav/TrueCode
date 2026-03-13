using Carter;
using TrueCode.Core.Models;

namespace TrueCode.FinanceService.Api.Endpoints;

public abstract class BaseEndpoint : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);
    
    protected static IResult ErrorResponse(IEnumerable<Error> errors)
    {
        return Results.Json(new { Errors = errors });
    }
}