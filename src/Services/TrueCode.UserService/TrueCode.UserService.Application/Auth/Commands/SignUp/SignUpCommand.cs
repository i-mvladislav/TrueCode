namespace TrueCode.UserService.Application.Auth.Commands.SignUp;

public sealed record SignUpCommand
{
    public required string Name { get; init; }
    public required string Password { get; init; }
}