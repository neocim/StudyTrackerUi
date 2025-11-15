using System.Reflection;
using Auth0.OidcClient;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudyTrackerUi.Api;

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
        builder.Services.AddTransient<Auth0Client>();
        builder.Services.AddHttpClient<ApiClient>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}