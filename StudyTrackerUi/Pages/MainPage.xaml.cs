namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnClicked(object? sender, EventArgs e)
    {
        Button.Text = "Clicked";
    }
}