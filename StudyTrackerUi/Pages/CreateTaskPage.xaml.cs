using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using StudyTrackerUi.Api;
using StudyTrackerUi.Pages.Common.Popups;
using StudyTrackerUi.Views;

namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CreateTaskViewModel _viewModel;

    public CreateTaskPage(ApiClient apiClient)
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
                    "Can not create a new task"), new PopupOptions { Shadow = null },
                CancellationToken.None);
            return;
        }

        // here we should get an user id from id token claim
        var userId = Guid.Parse("0556cb2d-4d72-4503-81ee-cd91116341b0");
        var taskId = Guid.NewGuid();
        var name = _viewModel.Name;
        var description = _viewModel.Description;
        var beginDate = _viewModel.BeginDate;
        var endDate = _viewModel.EndDate;

        var result =
            await _apiClient.CreateTask(userId, taskId, name, description,
                DateOnly.FromDateTime(beginDate), DateOnly.FromDateTime(endDate));

        if (result.IsError)
            await DisplayAlert($"Error: {result.Errors[0].Code}", result.Errors[0].Description,
                "Oh no!");

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}