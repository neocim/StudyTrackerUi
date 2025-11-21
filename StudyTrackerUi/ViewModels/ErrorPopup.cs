namespace StudyTrackerUi.ViewModels;

public sealed class ErrorPopupViewModel
{
    public ErrorPopupViewModel(string description, string? title = null)
    {
        ErrorTitle = title is null ? "Error" : $"Error {title}";
        ErrorDescription = description;
    }

    public string ErrorTitle { get; }
    public string ErrorDescription { get; }
}