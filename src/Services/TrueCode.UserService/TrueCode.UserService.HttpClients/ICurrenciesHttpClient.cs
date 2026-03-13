using Refit;

namespace TrueCode.UserService.HttpClients;

public interface ICurrenciesHttpClient
{
    [Get("/api/currencies/favorites")]
    public Task<ApiResponse<List<string>>> GetFavoriteCurrenciesAsync(Guid userId, [Authorize] string token, CancellationToken cancellationToken = default);
}