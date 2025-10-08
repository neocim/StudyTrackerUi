using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Pages.Common.Popups;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void CreateTasksButtonClicked(object? sender, EventArgs e)
    {
        var action = await this.ShowPopupAsync<string>(new TaskCreate(), new PopupOptions
            {
                Shadow = null,
                Shape = new RoundRectangle
                {
                    CornerRadius = 16
                }
            },
            CancellationToken.None);

        switch (action.Result)
        {
            case "Task":
            {
                await Navigation.PushAsync(new CreateTaskPage());
                break;
            }
            case "Subtask":
            {
                await Navigation.PushAsync(new CreateSubTaskPage());
                break;
            }
        }
    }
}