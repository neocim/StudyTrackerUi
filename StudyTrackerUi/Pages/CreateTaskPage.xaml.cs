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
        // NameEntry.Placeholder = "The name cannot be empty";
        // NameEntry.PlaceholderColor = (Color)Application.Current.Resources["Red"];
        CreateTaskButton.Text = $"{_viewModel.Name}";
    }
}