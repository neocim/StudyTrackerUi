using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;
using StudyTrackerUi.Web.Security;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    private readonly IMemoryCache _memoryCache;
    private readonly MainViewModel _viewModel;

    public MainPage(ApiClient apiClient, AuthService authService, IMemoryCache memoryCache)
    {
        InitializeComponent();
        BindingContext = new MainViewModel(apiClient, authService);
        _viewModel = (MainViewModel)BindingContext;
        _memoryCache = memoryCache;
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
                    await Navigation.PushAsync(new CreateTaskPage(_viewModel.ApiClient));
                    break;
                }
                case "Subtask":
                {
                    if (!_viewModel.CanCreateSubTasks)
                    {
                        await this.ShowPopupAsync(new ErrorPopup(
                                _viewModel.ErrorMessage!,
                                _viewModel.ErrorTitle), new PopupOptions { Shadow = null },
                            CancellationToken.None);
                        return;
                    }

                    await Navigation.PushAsync(new CreateSubTaskPage(_viewModel.ApiClient,
                        _memoryCache));
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