using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Extensions.Caching.Memory;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.Services.Security;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;

namespace StudyTrackerUi.Pages;

public partial class CreateSubTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CreateSubTaskViewModel _viewModel;

    public CreateSubTaskPage(ApiClient apiClient, IMemoryCache memoryCache)
    {
        InitializeComponent();
        BindingContext = new CreateSubTaskViewModel(apiClient, memoryCache);
        _viewModel = (CreateSubTaskViewModel)BindingContext;
        _apiClient = apiClient;
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

        // if there is still error after all checks above throw unexpected error
        if (_viewModel.ErrorMessage is not null)
        {
            await DisplayAlert("Unexpected error", _viewModel.ErrorMessage, "Oh no");
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

        var result =
            await _apiClient.CreateSubTask(tokenInfo.GetUserIdFromClaim(), parentTaskId.Value, name,
                description,
                DateOnly.FromDateTime(beginDate), DateOnly.FromDateTime(endDate));

        if (result.IsError)
            await DisplayAlert($"Error: {result.Errors[0].Code}",
                result.Errors[0].Description,
                "Oh no");

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}