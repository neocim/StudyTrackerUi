using Auth0.OidcClient;
using ErrorOr;
using Microsoft.Extensions.Configuration;

namespace StudyTrackerUi.Web.Security;

public sealed class AuthService
{
    private readonly Auth0Client _auth0Client;
    private readonly IConfiguration _configuration;
    private readonly SessionService _sessionService;

    public AuthService(Auth0Client auth0Client, IConfiguration configuration)
    {
        _auth0Client = auth0Client;
        _configuration = configuration;
        _sessionService = SessionService.Instance;
    }

    public async Task<ErrorOr<BearerTokenInfo>> Login()
    {
        if (await _sessionService.TokenExistsAsync())
        {
            if (await _sessionService.TokenExpiredAsync())
            {
                var refreshResult =
                    await RefreshTokenAsync(_sessionService.BearerTokenInfo!.RefreshToken);
                if (refreshResult.IsError) return refreshResult.Errors[0];

                await _sessionService.SaveBearerTokenInfoAsync(refreshResult.Value);
                return refreshResult.Value;
            }

            return _sessionService.BearerTokenInfo!;
        }

        var authResult = await Authenticate();
        if (authResult.IsError) return authResult.Errors[0];

        await _sessionService.SaveBearerTokenInfoAsync(authResult.Value);
        return authResult.Value;
    }

    private async Task<ErrorOr<BearerTokenInfo>> Authenticate()
    {
        var result =
            await _auth0Client.LoginAsync(new { audience = _configuration["Auth0:Audience"] });
        if (result.IsError)
            return Error.Custom(999, result.Error, result.ErrorDescription);

        var bearerTokenInfo = new BearerTokenInfo(
            result.AccessToken,
            result.IdentityToken,
            result.RefreshToken,
            result.AccessTokenExpiration);

        return bearerTokenInfo;
    }

    private async Task<ErrorOr<BearerTokenInfo>> RefreshTokenAsync(string refreshToken)
    {
        var result = await _auth0Client.RefreshTokenAsync(refreshToken);
        if (result.IsError)
            return Error.Custom(999, result.Error, result.ErrorDescription);

        var bearerTokenInfo = new BearerTokenInfo(
            result.AccessToken,
            result.IdentityToken,
            result.RefreshToken,
            result.AccessTokenExpiration);

        return bearerTokenInfo;
    }
}