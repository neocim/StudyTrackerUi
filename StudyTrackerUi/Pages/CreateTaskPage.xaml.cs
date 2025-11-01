using StudyTrackerUi.Views;

namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    private readonly CreateTaskViewModel _viewModel;

    public CreateTaskPage()
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
        _viewModel = (CreateTaskViewModel)BindingContext;
    }

    private void CreateButtonClicked(object? sender, EventArgs e)
    {
        if (!_viewModel.NameIsValid)
            return;

        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}