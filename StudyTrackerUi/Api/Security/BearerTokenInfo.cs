using System.Security.Claims;

namespace StudyTrackerUi.Api.Security;

public sealed class BearerTokenInfo
{
    public string AccessToken;
    public DateTimeOffset AccessTokenExpiration;
    public IEnumerable<Claim> Claims;
    public string RefreshToken;

    public BearerTokenInfo(string accessToken, string refreshToken,
        DateTimeOffset accessTokenExpiration, IEnumerable<Claim> claims)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        AccessTokenExpiration = accessTokenExpiration;
        Claims = claims;
    }

    public Guid GetUserIdFromClaim()
    {
        return Guid.Parse(Claims.FirstOrDefault(c => c.Type == "https://study.tracker.org/userId")!
            .Value);
    }
}