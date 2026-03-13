namespace TrueCode.Core.Users;

public interface ICurrentUserContext
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? Authorization { get; }
}