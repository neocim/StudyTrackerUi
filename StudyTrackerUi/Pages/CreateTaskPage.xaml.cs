using StudyTrackerUi.Api;
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

        var userId = Guid.Parse("0556cb2d-4d72-4503-81ee-cd91116341b0");
        var taskId = Guid.NewGuid();
        var name = _viewModel.Name;
        var description = _viewModel.Description;
        var beginDate = _viewModel.BeginDate;
        var endDate = _viewModel.EndDate;

        var result =
            await _apiClient.CreateTask(userId, taskId, name, description, beginDate, endDate);

        if (result.IsError)
        {
            CreateTaskButton.Text = $"{result.Errors[0].Code}";
            return;
        }

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}