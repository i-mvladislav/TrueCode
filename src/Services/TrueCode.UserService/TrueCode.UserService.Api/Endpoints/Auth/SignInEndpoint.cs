using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Auth.Commands.SignIn;

namespace TrueCode.UserService.Api.Endpoints.Auth;

public class SignInEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/signIn", HandlePost);
    }
    
    private async Task<IResult> HandlePost(HttpContext ctx,
        SignInRequest request,
        SignInCommandHandler commandHandler)
    {
        var command = new SignInCommand
        {
            Name = request.Name,
            Password = request.Password
        };

        var result = await commandHandler.ExecuteAsync(command, ctx.RequestAborted);

        if (!result.IsSuccess)
        {
            return ErrorResponse(result.Errors);
        }
        
        return Results.Json(result.Data);
    }
}