using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.Services;
using StudyTrackerUi.Services.Security;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;

namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CacheService _cacheService;
    private readonly CreateTaskViewModel _viewModel;

    public CreateTaskPage(ApiClient apiClient, CacheService cacheService)
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
        _viewModel = (CreateTaskViewModel)BindingContext;
        _apiClient = apiClient;
        _cacheService = cacheService;
    }

    private async void CreateButtonClicked(object? sender, EventArgs e)
    {
        if (!_viewModel.NameIsValid)
            return;

        if (!_viewModel.DateIsValid)
        {
            await this.ShowPopupAsync(new ErrorPopup(_viewModel.ErrorMessage!,
                    "Can not create a new task"), new PopupOptions { Shadow = null },
                CancellationToken.None);
            return;
        }

        // if there is still error after all checks above throw unexpected error
        if (_viewModel.ErrorMessage is not null)
        {
            await DisplayAlert("Unexpected error", _viewModel.ErrorMessage, "Oh no");
            return;
        }

        var tokenInfo = await SessionService.Instance.GetBearerTokenInfoAsync();
        if (tokenInfo is null)
        {
            await DisplayAlert("Unexpected authentication error", "Couldn't get bearer token info",
                "Oh no");
            return;
        }

        var name = _viewModel.Name;
        var description = _viewModel.Description;
        var beginDate = _viewModel.BeginDate;
        var endDate = _viewModel.EndDate;

        try
        {
            var result =
                await _apiClient.CreateTask(tokenInfo.GetUserIdFromClaim(), name,
                    description,
                    DateOnly.FromDateTime(beginDate), DateOnly.FromDateTime(endDate));

            if (result.IsError)
                await DisplayAlert($"Error: {result.Errors[0].Code}",
                    result.Errors[0].Description,
                    "Oh no");

            // REMOVE IT; REDIRECT TO TASK EDIT PAGE INSTEAD
            CreateTaskButton.Text = $"{_viewModel.Name}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Couldn't create a task", ex.Message, "Oh no");
        }

        try
        {
            var result = await _apiClient.GetTasks(tokenInfo.GetUserIdFromClaim());

            if (result.IsError)
                await DisplayAlert($"Error: {result.Errors[0].Code}",
                    result.Errors[0].Description,
                    "Oh no");

            _cacheService.SetTasks(result.Value);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Failed to get tasks", ex.Message, "Oh no");
        }
    }
}