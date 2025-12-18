using System.IdentityModel.Tokens.Jwt;

namespace StudyTrackerUi.Services.Security;

public sealed class BearerTokenInfo
{
    public string AccessToken;
    public DateTimeOffset AccessTokenExpiration;
    public string IdToken;
    public string RefreshToken;

    public BearerTokenInfo(string accessToken, string idToken, string refreshToken,
        DateTimeOffset accessTokenExpiration)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        AccessTokenExpiration = accessTokenExpiration;
        IdToken = idToken;
    }

    public Guid GetUserIdFromClaim()
    {
        return Guid.Parse(new JwtSecurityTokenHandler().ReadJwtToken(IdToken).Claims
            .FirstOrDefault(c => c.Type == "https://study.tracker.org/userId")!
            .Value);
    }
}