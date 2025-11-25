using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using StudyTrackerUi.Api;
using StudyTrackerUi.Api.Security;

namespace StudyTrackerUi.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private string _errorMessage;
    private bool _hasAuthError;
    public ApiClient ApiClient;
    public AuthService AuthService;

    public MainViewModel(ApiClient apiClient, AuthService authService)
    {
        ApiClient = apiClient;
        AuthService = authService;
        LoginCommand = new AsyncRelayCommand(Login);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasAuthError
    {
        get => _hasAuthError;
        set
        {
            if (_hasAuthError != value)
            {
                _hasAuthError = value;
                OnPropertyChanged();
            }
        }
    }

    public IAsyncRelayCommand LoginCommand { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task Login()
    {
        var result = await AuthService.Login();

        if (result.IsError)
        {
            HasAuthError = true;
            ErrorMessage = result.Errors[0].Description;

            return;
        }

        ApiClient.SetAuthHeader(result.Value.AccessToken);
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}