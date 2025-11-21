using Newtonsoft.Json;

namespace StudyTrackerUi.Api.Security;

public sealed class SessionService
{
    public static SessionService Instance = new();
    public readonly string StorageKey = "BearerTokenInfo";
    public BearerTokenInfo BearerTokenInfo { get; private set; } = null!;

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