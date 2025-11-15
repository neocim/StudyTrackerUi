using Android.App;
using Android.Content;
using Android.Content.PM;

namespace StudyTrackerUi;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
                           ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize |
                           ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "org.study.tracker", DataHost = "study.tracker.org",
    DataPathPrefix = "/android/org.study.tracker/callback")]
public class MainActivity : MauiAppCompatActivity
{
}