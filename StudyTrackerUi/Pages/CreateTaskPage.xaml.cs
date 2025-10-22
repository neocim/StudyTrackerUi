namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    public CreateTaskPage()
    {
        InitializeComponent();
    }

    private void CreateButtonClicked(object? sender, EventArgs e)
    {
        CreateTaskButton.Text = "Clicked";
    }

    private void BeginDateSelected(object sender, DateChangedEventArgs e)
    {
    }
}