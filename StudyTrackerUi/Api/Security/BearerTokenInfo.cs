namespace StudyTrackerUi.Api.Security;

public sealed class BearerTokenInfo(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiration
);