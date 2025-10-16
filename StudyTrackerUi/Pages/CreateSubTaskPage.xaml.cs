namespace StudyTrackerUi.Pages;

public partial class CreateSubTaskPage : ContentPage
{
    public CreateSubTaskPage()
    {
        InitializeComponent();
    }

    private void CreateButtonClicked(object? sender, EventArgs e)
    {
        CreateButton.Text = "Clicked";
    }

    private void BeginDateSelected(object sender, DateChangedEventArgs e)
    {
    }
}