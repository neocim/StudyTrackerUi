namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void RainbowOnClicked(object? sender, EventArgs e)
    {
        Button.Text = "Rainbow on";
    }
}