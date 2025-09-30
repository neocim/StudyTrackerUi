namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void CreateTaskButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateTaskPage());
    }
}