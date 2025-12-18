using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.Services;
using StudyTrackerUi.Services.Security;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    private readonly CacheService _cacheService;
    private readonly MainViewModel _viewModel;

    public MainPage(ApiClient apiClient, AuthService authService, CacheService cacheService)
    {
        InitializeComponent();
        BindingContext = new MainViewModel(apiClient, authService, cacheService);
        _viewModel = (MainViewModel)BindingContext;
        _cacheService = cacheService;
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            await _viewModel.CheckUserTasks();
            await _viewModel.Login();
        }
        catch (Exception ex)
        {
            _viewModel.ErrorTitle = ex.HResult.ToString();
            _viewModel.ErrorMessage = ex.Message;
        }
    }

    private async void CreateTasksButtonClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_viewModel.ErrorMessage is not null)
                await DisplayAlert(_viewModel.ErrorTitle, _viewModel.ErrorMessage, "Oh no");

            if (await SessionService.Instance.GetBearerTokenInfoAsync() is null)
            {
                await this.ShowPopupAsync(new ErrorPopup(
                        "To create a task, you must log in to your account.",
                        "Log in, please"), new PopupOptions { Shadow = null },
                    CancellationToken.None);
                return;
            }

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
                    await Navigation.PushAsync(new CreateTaskPage(_viewModel.ApiClient,
                        _cacheService));
                    break;
                }
                case "Subtask":
                {
                    if (!_viewModel.CanCreateSubTasks)
                    {
                        await this.ShowPopupAsync(new ErrorPopup(
                                "First, you need to create at least one task for which you want to create a subtask.",
                                "Couldn't create a subtask"),
                            new PopupOptions { Shadow = null },
                            CancellationToken.None);
                        return;
                    }

                    await Navigation.PushAsync(new CreateSubTaskPage(_viewModel.ApiClient,
                        _cacheService));
                    break;
                }
            }
        }
        catch (Exception exception)
        {
            await DisplayAlert("Unexpected error", exception.Message, "Oh no");
        }
    }
}