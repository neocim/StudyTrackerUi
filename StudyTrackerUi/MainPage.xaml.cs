namespace StudyTrackerUi;

public partial class MainPage : ContentPage
{
    private static readonly Random _rnd = new();
    private static bool _clicked;
    private Timer _timer;

    public MainPage()
    {
        InitializeComponent();
    }

    public static (int, int, int) RandomRgb()
    {
        var r = _rnd.Next(0, 256);
        var g = _rnd.Next(0, 256);
        var b = _rnd.Next(0, 256);
        return (r, g, b);
    }

    private void RainbowOnClicked(object? sender, EventArgs e)
    {
        if (!_clicked)
        {
            var timerCallback = new TimerCallback(_ =>
            {
                var color = RandomRgb();
                var rgbResult = Color.FromRgb(color.Item1, color.Item2, color.Item3);
                Btn.Text = "Clicked!";
                Btn.BackgroundColor = rgbResult;
                Btn.BorderColor = rgbResult;
                SemanticScreenReader.Announce(Btn.Text);
            });

            timerCallback(null);

            _timer = new Timer(timerCallback, null, 0, 500);
            _clicked = true;
        }
    }
}