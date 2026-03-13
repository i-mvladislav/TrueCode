using Microsoft.AspNetCore.Mvc;
using TrueCode.Core.Commands;
using TrueCode.UserService.Api.RequestDtos;
using TrueCode.UserService.Application.Auth.Commands.SignIn;
using TrueCode.UserService.Application.Auth.Models;

namespace TrueCode.UserService.Api.Endpoints.Auth;

public class SignInEndpoint : BaseEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/signIn", HandlePost);
    }
    
    private async Task<IResult> HandlePost(HttpContext ctx,
        [FromBody] SignInRequest request,
        [FromServices] BaseCommandHandler<SignInCommand, JwtToken> commandHandler,
        [FromServices] ILogger<SignInEndpoint> logger)
    {
        var command = new SignInCommand
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