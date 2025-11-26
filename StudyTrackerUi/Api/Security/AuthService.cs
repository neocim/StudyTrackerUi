using System.IdentityModel.Tokens.Jwt;
using Auth0.OidcClient;
using ErrorOr;

namespace StudyTrackerUi.Api.Security;

public sealed class AuthService
{
    private readonly Auth0Client _auth0Client;
    private readonly SessionService _sessionService;

    public AuthService(Auth0Client auth0Client)
    {
        _auth0Client = auth0Client;
        _sessionService = SessionService.Instance;
    }

    public async Task<ErrorOr<BearerTokenInfo>> Login()
    {
        if (await _sessionService.TokenExistsAsync())
        {
            if (_sessionService.TokenExpired())
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
        var result = await _auth0Client.LoginAsync();
        if (result.IsError)
            return Error.Custom(999, result.Error, result.ErrorDescription);

        var bearerTokenInfo = new BearerTokenInfo(result.AccessToken, result.RefreshToken,
            result.AccessTokenExpiration,
            result.User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value))
        );

        return bearerTokenInfo;
    }

    private async Task<ErrorOr<BearerTokenInfo>> RefreshTokenAsync(string refreshToken)
    {
        var result = await _auth0Client.RefreshTokenAsync(refreshToken);
        if (result.IsError)
            return Error.Custom(999, result.Error, result.ErrorDescription);

        var tokenHandler = new JwtSecurityTokenHandler();

        var bearerTokenInfo = new BearerTokenInfo(result.AccessToken, result.RefreshToken,
            result.AccessTokenExpiration,
            tokenHandler.ReadJwtToken(result.IdentityToken).Claims
                .Select(c => new KeyValuePair<string, string>(c.Type, c.Value))
        );

        return bearerTokenInfo;
    }
}