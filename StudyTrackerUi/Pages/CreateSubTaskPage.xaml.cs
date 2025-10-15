namespace StudyTrackerUi.Pages;

public partial class CreateSubTaskPage : ContentPage
{
    public CreateSubTaskPage()
    {
        InitializeComponent();
    }

    private void OnClicked(object? sender, EventArgs e)
    {
        Button.Text = "Clicked";
    }

    private void SubtaskBeginDateSelected(object sender, DateChangedEventArgs e)
    {
    }
}