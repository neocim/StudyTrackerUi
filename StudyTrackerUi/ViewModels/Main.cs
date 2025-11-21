using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;

namespace StudyTrackerUi.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public ApiClient ApiClient;
    public AuthService AuthService;
    public SessionService SessionService;

    public MainViewModel(ApiClient apiClient, AuthService authService,
        SessionService sessionService)
    {
        ApiClient = apiClient;
        AuthService = authService;
        SessionService = sessionService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}