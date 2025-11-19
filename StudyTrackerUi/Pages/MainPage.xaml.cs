using Auth0.OidcClient;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Shapes;
using StudyTrackerUi.Api;
using StudyTrackerUi.Pages.Common.Popups;

namespace StudyTrackerUi.Pages;

public partial class MainPage : ContentPage
{
    private readonly ApiClient _apiClient;
    private readonly Auth0Client _auth0Client;

    public MainPage(IConfiguration configuration, ApiClient apiClient)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _auth0Client = new Auth0Client(new Auth0ClientOptions
        {
            Domain = configuration["Auth0:Domain"],
            ClientId = configuration["Auth0:ClientId"]
        });
    }

    private async void CreateTasksButtonClicked(object? sender, EventArgs e)
    {
        var action = await this.ShowPopupAsync<string>(new TaskCreate(), new PopupOptions
            {
                Shadow = null,
                Shape = new RoundRectangle
                {
                    CornerRadius = 16
                }
            },
            CancellationToken.None);

        switch (action.Result)
        {
            case "Task":
            {
                await Navigation.PushAsync(new CreateTaskPage(_apiClient));
                break;
            }
            case "Subtask":
            {
                await Navigation.PushAsync(new CreateSubTaskPage());
                break;
            }
        }
    }
}