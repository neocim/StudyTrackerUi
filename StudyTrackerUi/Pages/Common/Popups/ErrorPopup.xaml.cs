using CommunityToolkit.Maui.Views;
using StudyTrackerUi.ViewModels;

namespace StudyTrackerUi.Pages.Common.Popups;

public partial class ErrorPopup : Popup
{
    public ErrorPopup(string errorDescription, string? errorTitle = null)
    {
        InitializeComponent();

        BindingContext = new ErrorPopupViewModel(errorDescription, errorTitle);
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}