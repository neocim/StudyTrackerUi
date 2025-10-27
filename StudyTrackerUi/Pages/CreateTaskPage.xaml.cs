using StudyTrackerUi.Views;

namespace StudyTrackerUi.Pages;

public partial class CreateTaskPage : ContentPage
{
    public CreateTaskPage()
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
    }

    private void CreateButtonClicked(object? sender, EventArgs e)
    {
        var viewModel = (CreateTaskViewModel)BindingContext;
        CreateTaskButton.Text = $"{viewModel.Name}";
    }
}