using System.Reflection;
using Auth0.OidcClient;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;

namespace StudyTrackerUi;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        using var appSettingsStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("StudyTrackerUi.appsettings.json")!;

        var config = new ConfigurationBuilder()
            .AddJsonStream(appSettingsStream)
            .AddEnvironmentVariables()
            .Build();

        builder.Configuration.AddConfiguration(config);
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ApiRequest>();
        builder.Services.AddHttpClient<ApiClient>();
        builder.Services.AddScoped<Auth0Client>(_ => new Auth0Client(new Auth0ClientOptions
        {
            Domain = builder.Configuration["Auth0:Domain"],
            ClientId = builder.Configuration["Auth0:ClientId"],
            RedirectUri = builder.Configuration["Auth0:RedirectUri"]
        }));
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<SessionService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}