namespace StudyTrackerUi.Api.Security;

public sealed class BearerTokenInfo
{
    public string AccessToken;
    public DateTimeOffset AccessTokenExpiration;
    public string RefreshToken;

    public BearerTokenInfo(string accessToken, string refreshToken,
        DateTimeOffset accessTokenExpiration)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        AccessTokenExpiration = accessTokenExpiration;
    }
}