namespace TrueCode.UserService.Api.RequestDtos;

public sealed record SignUpRequest
{
    public required string Name { get; init; }
    public required string Password { get; init; }
}