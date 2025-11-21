using Auth0.OidcClient;
using ErrorOr;

namespace StudyTrackerUi.Api.Security;

public sealed class AuthService
{
    private readonly Auth0Client _auth0Client;

    public AuthService(Auth0Client auth0Client)
    {
        _auth0Client = auth0Client;
    }

    public async Task<ErrorOr<BearerTokenInfo>> Authenticate()
    {
        var result = await _auth0Client.LoginAsync();
        if (result.IsError)
            return Error.Custom(999, result.Error, result.ErrorDescription);

        var bearerTokenInfo = new BearerTokenInfo(result.AccessToken, result.RefreshToken,
            result.AccessTokenExpiration
        );

        return bearerTokenInfo;
    }
}