using Newtonsoft.Json;

namespace StudyTrackerUi.Services.Security;

public sealed class SessionService
{
    public static readonly SessionService Instance = new();
    public readonly string StorageKey = "BearerTokenInfo";
    public BearerTokenInfo? BearerTokenInfo { get; private set; }

    public async Task SaveBearerTokenInfoAsync(BearerTokenInfo bearerTokenInfo)
    {
        BearerTokenInfo = bearerTokenInfo;
        var token = JsonConvert.SerializeObject(bearerTokenInfo);
        await SecureStorage.Default.SetAsync(StorageKey, token);
    }

    public async Task<BearerTokenInfo?> GetBearerTokenInfoAsync()
    {
        if (await SecureStorage.Default.GetAsync(StorageKey) is { } token)
        {
            BearerTokenInfo = JsonConvert.DeserializeObject<BearerTokenInfo>(token);
            return BearerTokenInfo;
        }

        return null;
    }

    public async Task<bool> TokenExistsAsync()
    {
        var tokenString = await SecureStorage.Default.GetAsync(StorageKey);
        return !string.IsNullOrEmpty(tokenString);
    }

    public async Task<bool> TokenExpiredAsync()
    {
        var token = await GetBearerTokenInfoAsync();
        if (token is null) return true;

        return DateTimeOffset.UtcNow > token.AccessTokenExpiration;
    }

    public void ClearStorage()
    {
        SecureStorage.Default.RemoveAll();
    }
}