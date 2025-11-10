using ErrorOr;
using StudyTrackerUi.Api;
using StudyTrackerUi.Views;

namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly CreateTaskViewModel _viewModel;

    public CreateTaskPage()
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
        _viewModel = (CreateTaskViewModel)BindingContext;
    }

    protected void Error(List<Error> errors)
    {
    }

    private void CreateButtonClicked(object? sender, EventArgs e)
    {
        if (!_viewModel.NameIsValid)
            return;

        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var name = _viewModel.Name;
        var description = _viewModel.Description;
        var beginDate = _viewModel.BeginDate;
        var endDate = _viewModel.EndDate;

        var result = _apiClient.CreateTask(userId, taskId, name, description, beginDate, endDate);

        if (result.Result.IsError)
        {
            CreateTaskButton.Text = $"{result.Result.Errors[0].Code}";
            return;
        }

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}