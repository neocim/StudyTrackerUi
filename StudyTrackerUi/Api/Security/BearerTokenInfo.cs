namespace StudyTrackerUi.Api.Security;

public sealed class BearerTokenInfo
{
    public string AccessToken;
    public DateTimeOffset AccessTokenExpiration;
    public IEnumerable<KeyValuePair<string, string>> Claims;
    public string RefreshToken;

    public BearerTokenInfo(string accessToken, string refreshToken,
        DateTimeOffset accessTokenExpiration, IEnumerable<KeyValuePair<string, string>> claims)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        AccessTokenExpiration = accessTokenExpiration;
        Claims = claims;
    }

    public Guid GetUserIdFromClaim()
    {
        return Guid.Parse(Claims.FirstOrDefault(c => c.Key == "https://study.tracker.org/userId")
            .Value);
    }
}