using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyTrackerUi.Web;
using StudyTrackerUi.Web.Security;

namespace StudyTrackerUi.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    public readonly ApiClient ApiClient;
    private bool _canCreateSubTasks;
    private string? _errorMessage;
    private string? _errorTitle;

    public MainViewModel(ApiClient apiClient, AuthService authService)
    {
        ApiClient = apiClient;
        _authService = authService;
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage == value) return;
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public string? ErrorTitle
    {
        get => _errorTitle;
        set
        {
            if (_errorTitle == value) return;
            _errorTitle = value;
            OnPropertyChanged();
        }
    }

    public bool CanCreateSubTasks
    {
        get => _canCreateSubTasks;
        set
        {
            if (_canCreateSubTasks == value) return;
            _canCreateSubTasks = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task CheckUserTasks()
    {
        var tokenInfo = await SessionService.Instance.GetBearerTokenInfoAsync();
        if (tokenInfo is null)
        {
            ErrorTitle = "Unexpected authentication error";
            ErrorMessage = "Couldn't get bearer token info";
            return;
        }

        var result = await ApiClient.GetTasks(tokenInfo.GetUserIdFromClaim());

        if (result.IsError)
        {
            ErrorTitle = "Coludn't get get user tasks info";
            ErrorMessage = result.Errors[0].Description;
        }

        CanCreateSubTasks = result.Value.Any();
    }

    public async Task Login()
    {
        var result = await _authService.Login();

        if (result.IsError)
        {
            ErrorTitle = result.Errors[0].Code;
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