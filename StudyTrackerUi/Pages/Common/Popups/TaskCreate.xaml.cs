using CommunityToolkit.Maui.Views;

namespace StudyTrackerUi.Pages.Common.Popups;

public partial class TaskCreate : Popup<string>
{
    public TaskCreate()
    {
        InitializeComponent();
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await CloseAsync(button.Text);
    }
}