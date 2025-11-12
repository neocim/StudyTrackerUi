using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Api;
using StudyTrackerUi.Pages.Common.Popups;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    private readonly ApiClient _apiClient;

    public MainPage(ApiClient apiClient)
    {
        InitializeComponent();
        _apiClient = apiClient;
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
                await Navigation.PushAsync(new CreateTaskPage(_apiClient));
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