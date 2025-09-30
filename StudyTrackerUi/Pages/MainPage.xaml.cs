namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void CreateTaskButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateTaskPage());
    }
}