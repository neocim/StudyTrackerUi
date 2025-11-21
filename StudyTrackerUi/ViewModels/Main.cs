using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;

namespace StudyTrackerUi.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private ApiClient _apiClient;
    private AuthService _authService;
    private SessionService _sessionService;

    public MainViewModel(ApiClient apiClient, AuthService authService,
        SessionService sessionService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _sessionService = sessionService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}