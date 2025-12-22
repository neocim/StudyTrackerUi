using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.Services;
using StudyTrackerUi.Services.Security;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;

namespace StudyTrackerUi.Pages;

public partial class CreateSubTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CacheService _cacheService;
    private readonly CreateSubTaskViewModel _viewModel;

    public CreateSubTaskPage(ApiClient apiClient, CacheService cacheService)
    {
        InitializeComponent();
        BindingContext = new CreateSubTaskViewModel(apiClient, cacheService);
        _viewModel = (CreateSubTaskViewModel)BindingContext;
        _apiClient = apiClient;
        _cacheService = cacheService;
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            await _viewModel.GetExistingTasks();
        }
        catch (Exception ex)
        {
            _viewModel.UnexpectedErrorMessage = ex.Message;
        }
    }

    private async void CreateButtonClicked(object? sender, EventArgs e)
    {
        try
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

            if (!string.IsNullOrEmpty(_viewModel.UnexpectedErrorMessage))
            {
                await DisplayAlert("Unexpected error", _viewModel.ErrorMessage, "Oh no");
                _viewModel.UnexpectedErrorMessage = string.Empty;
                return;
            }

            var parentTaskId = _viewModel.SelectedTaskId;
            if (parentTaskId is null)
            {
                await this.ShowPopupAsync(new ErrorPopup(
                        "Please, select the task for which you want to create a subtask",
                        "Can not create a new task"),
                    new PopupOptions { Shadow = null },
                    CancellationToken.None);
                return;
            }

            var tokenInfo = await SessionService.Instance.GetBearerTokenInfoAsync();
            if (tokenInfo is null)
            {
                await DisplayAlert("Unexpected error", "Couldn't get bearer token info", "Oh no");
                return;
            }

            var name = _viewModel.Name;
            var description = _viewModel.Description;
            var beginDate = _viewModel.BeginDate;
            var endDate = _viewModel.EndDate;

            try
            {
                var result =
                    await _apiClient.CreateSubTask(tokenInfo.GetUserIdFromClaim(),
                        parentTaskId.Value,
                        name,
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
                await DisplayAlert("Couldn't create a subtask", ex.Message, "Oh no");
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
        catch (Exception exception)
        {
            await DisplayAlert("Unexpected error", exception.Message, "Oh no");
        }
    }
}