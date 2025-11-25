using Newtonsoft.Json;

namespace StudyTrackerUi.Api.Security;

public sealed class SessionService
{
    public static SessionService Instance = new();
    public readonly string StorageKey = "BearerTokenInfo";
    public BearerTokenInfo? BearerTokenInfo { get; private set; }

    public async Task SaveBearerTokenInfoAsync(BearerTokenInfo bearerTokenInfo)
    {
        BearerTokenInfo = bearerTokenInfo;
        var token = JsonConvert.SerializeObject(bearerTokenInfo);
        await SecureStorage.Default.SetAsync(StorageKey, token);
    }

    public async Task<bool> TokenExistsAsync()
    {
        if (BearerTokenInfo is null) return false;

        var tokenString = await SecureStorage.Default.GetAsync(StorageKey);
        if (string.IsNullOrEmpty(tokenString)) return false;

        BearerTokenInfo = JsonConvert.DeserializeObject<BearerTokenInfo>(tokenString);
        return true;
    }

    public void ClearStorage()
    {
        SecureStorage.Default.RemoveAll();
    }

    public bool TokenExpired()
    {
        return DateTimeOffset.UtcNow > BearerTokenInfo!.AccessTokenExpiration;
    }
}