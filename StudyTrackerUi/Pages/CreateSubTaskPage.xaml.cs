using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.ViewModels;
using StudyTrackerUi.Web;
using StudyTrackerUi.Web.Security;

namespace StudyTrackerUi.Pages;

public partial class СreateSubTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CreateTaskViewModel _viewModel;

    public СreateSubTaskPage(ApiClient apiClient)
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
        _viewModel = (CreateTaskViewModel)BindingContext;
        _apiClient = apiClient;
    }

    private async void CreateButtonClicked(object? sender, EventArgs e)
    {
        if (!_viewModel.NameIsValid)
            return;

        if (!_viewModel.DateIsValid)
        {
            await this.ShowPopupAsync(new ErrorPopup(_viewModel.ErrorMessage,
                    "Can not create a new sub task"), new PopupOptions { Shadow = null },
                CancellationToken.None);
            return;
        }

        var tokenInfo = await SessionService.Instance.GetBearerTokenInfoAsync();
        if (tokenInfo is null)
        {
            await DisplayAlert("Unexpected error", "Couldn't get bearer token info", "Oh no!");
            return;
        }

        var REMOVEIT = Guid.NewGuid();
        var name = _viewModel.Name;
        var description = _viewModel.Description;
        var beginDate = _viewModel.BeginDate;
        var endDate = _viewModel.EndDate;

        try
        {
            var result =
                await _apiClient.CreateSubTask(tokenInfo.GetUserIdFromClaim(), REMOVEIT, name,
                    description,
                    DateOnly.FromDateTime(beginDate), DateOnly.FromDateTime(endDate));

            if (result.IsError)
                await DisplayAlert($"Error: {result.Errors[0].Code}", result.Errors[0].Description,
                    "Oh no!");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Unexpected error", exception.Message, "Oh no!");
        }

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}