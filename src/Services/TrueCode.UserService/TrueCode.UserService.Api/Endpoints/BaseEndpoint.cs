using Carter;
using TrueCode.Core.Models;

namespace TrueCode.UserService.Api.Endpoints;

public abstract class BaseEndpoint : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);

    protected static IResult ErrorResponse(IEnumerable<Error> errors)
    {
        return Results.Json(new { Errors = errors });
    }
}