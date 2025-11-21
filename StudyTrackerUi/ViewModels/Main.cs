using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using ErrorOr;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;

namespace StudyTrackerUi.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public ApiClient ApiClient;
    public AuthService AuthService;
    public SessionService SessionService;

    public MainViewModel(ApiClient apiClient, AuthService authService)
    {
        ApiClient = apiClient;
        AuthService = authService;
        SessionService = SessionService.Instance;
        LoginCommand = new RelayCommand(Login);
    }

    public IRelayCommand LoginCommand { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task<ErrorOr<Success>> Login()
    {
        if (!await SessionService.SessionValidAsync())
        {
            var result = await AuthService.Authenticate();
            if (result.IsError) return result.Errors[0];

            ApiClient.SetAuthHeader(result.Value.AccessToken);
        }

        return Result.Success;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}