using Microsoft.Extensions.Caching.Distributed;

namespace TrueCode.UserService.Api.Services;

public class JwtBlacklistService(IDistributedCache cache)
{
    private const string Key = "jwt-blacklist:";
    
    public async Task AddToken(string jti, TimeSpan expires)
    {
        await cache.SetStringAsync($"{Key}{jti}", "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expires
        });
    }

    public async Task<bool> HasToken(string jti)
    {
        var value = await cache.GetStringAsync($"{Key}{jti}");
        return value is not null;
    }
}