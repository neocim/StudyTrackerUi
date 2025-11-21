using Newtonsoft.Json;

namespace StudyTrackerUi.Api.Security;

public sealed class SessionService
{
    public static SessionService Instance = new();
    public readonly string StorageKey = "BearerTokenInfo";
    public BearerTokenInfo? BearerTokenInfo { get; private set; }

    public async Task<bool> TokenExistsAsync()
    {
        if (BearerTokenInfo is null) return false;

        var tokenString = await SecureStorage.Default.GetAsync(StorageKey);
        if (string.IsNullOrEmpty(tokenString)) return false;

        BearerTokenInfo = JsonConvert.DeserializeObject<BearerTokenInfo>(tokenString);
        return true;
    }

    public bool TokenExpired()
    {
        if (BearerTokenInfo is null) return true;

        return DateTimeOffset.UtcNow > BearerTokenInfo!.AccessTokenExpiration;
    }

    public async Task<bool> SessionValidAsync()
    {
        if (!await TokenExistsAsync()) return false;
        if (TokenExpired()) return false;

        return true;
    }

    public async Task<BearerTokenInfo?> GetBearerTokenInfo()
    {
        if (await SecureStorage.Default.GetAsync(StorageKey) is { } token)
            return JsonConvert.DeserializeObject<BearerTokenInfo>(token);

        return null;
    }

    public async Task SaveBearerTokenInfo(BearerTokenInfo bearerTokenInfo)
    {
        BearerTokenInfo = bearerTokenInfo;
        var token = JsonConvert.SerializeObject(bearerTokenInfo);
        await SecureStorage.Default.SetAsync(StorageKey, token);
    }
}