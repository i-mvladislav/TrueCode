using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Auth.Commands.SignUp;

namespace TrueCode.UserService.Api.Endpoints.Auth;

public class SignUpEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/signUp", HandlePost);
    }
    
    private async Task<IResult> HandlePost(HttpContext ctx,
        SignUpRequest request,
        SignUpCommandHandler commandHandler)
    {
        var command = new SignUpCommand
        {
            Name = request.Name,
            Password = request.Password
        };

        var result = await commandHandler.ExecuteAsync(command, ctx.RequestAborted);

        if (!result.IsSuccess)
        {
            return ErrorResponse(result.Errors);
        }
        
        return Results.Ok();
    }
}