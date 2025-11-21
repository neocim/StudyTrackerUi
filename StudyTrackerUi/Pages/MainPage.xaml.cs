using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.ViewModels;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _mainViewModel;

    public MainPage(ApiClient apiClient, AuthService authService)
    {
        InitializeComponent();
        BindingContext = new MainViewModel(apiClient, authService);
        _mainViewModel = (MainViewModel)BindingContext;
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
                await Navigation.PushAsync(new CreateTaskPage(_mainViewModel.ApiClient));
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