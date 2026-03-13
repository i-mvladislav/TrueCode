using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Commands;
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
        [FromBody] SignUpRequest request,
        [FromServices] BaseCommandHandler<SignUpCommand> commandHandler,
        [FromServices] ILogger<SignUpEndpoint> logger)
    {
        var command = new SignUpCommand
        {
            Name = request.Name,
            Password = request.Password
        };

        return await ExecuteCommandAsync(
            command,
            async cmd => await commandHandler.ExecuteAsync(command, ctx.RequestAborted),
            logger
        );
    }
}