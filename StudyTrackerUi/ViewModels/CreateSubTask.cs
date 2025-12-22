using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using StudyTrackerUi.Dto;
using StudyTrackerUi.Services;
using StudyTrackerUi.Services.Security;
using StudyTrackerUi.ViewModels.Validators;
using StudyTrackerUi.Web;
using Task = System.Threading.Tasks.Task;

namespace StudyTrackerUi.ViewModels;

public sealed class CreateSubTaskViewModel : INotifyPropertyChanged
{
    private readonly ApiClient _apiClient;
    private readonly CacheService _cacheService;
    private readonly SubTaskValidator _validator;
    private DateTime _beginDate;
    private bool _dateIsValid;
    private string _description;
    private DateTime _endDate;
    private string? _errorMessage;
    private IEnumerable<TaskNode> _existingTasks;
    private string _name;
    private bool _nameIsValid;
    private Guid? _selectedTaskId;
    private string? _unexpectedErrorMessage;

    public CreateSubTaskViewModel(ApiClient apiClient, CacheService cacheService)
    {
        _cacheService = cacheService;
        _apiClient = apiClient;
        _validator = new SubTaskValidator();
        _existingTasks = new List<TaskNode>();
        // without this, even if the user did not have time to enter anything, entry will be highlighted with an error
        _nameIsValid = true;
        _name = null!;
        _description = null!;
        _beginDate = DateTime.Now.Date;
        _endDate = DateTime.Now.Date.AddDays(1);

        ValidateCommand = new AsyncRelayCommand(Validate);
    }

    public IAsyncRelayCommand ValidateCommand { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            NameIsValid = true;
            OnPropertyChanged();
        }
    }

    public Guid? SelectedTaskId
    {
        get => _selectedTaskId;
        set
        {
            if (_selectedTaskId == value) return;
            _selectedTaskId = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description == value) return;
            _description = value;
            OnPropertyChanged();
        }
    }

    public DateTime BeginDate
    {
        get => _beginDate;
        set
        {
            if (_beginDate == value) return;
            _beginDate = value;
            OnPropertyChanged();
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate == value) return;
            _endDate = value;
            OnPropertyChanged();
        }
    }

    public bool NameIsValid
    {
        get => _nameIsValid;
        set
        {
            if (_nameIsValid == value) return;
            _nameIsValid = value;
            OnPropertyChanged();
        }
    }

    public bool DateIsValid
    {
        get => _dateIsValid;
        set
        {
            if (_dateIsValid == value) return;
            _dateIsValid = value;
            OnPropertyChanged();
        }
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

    public string? UnexpectedErrorMessage
    {
        get => _unexpectedErrorMessage;
        set
        {
            if (_unexpectedErrorMessage == value) return;
            _unexpectedErrorMessage = value;
            OnPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task GetExistingTasks()
    {
        var tokenInfo = await SessionService.Instance.GetBearerTokenInfoAsync();
        if (tokenInfo is null)
        {
            _unexpectedErrorMessage = "Couldn't get bearer token info";
            return;
        }

        var tasks = _cacheService.GetTasks();
        if (tasks.Any())
        {
            _existingTasks = tasks;
            return;
        }

        var result = await _apiClient.GetTasks(tokenInfo.GetUserIdFromClaim());

        if (result.IsError)
            _unexpectedErrorMessage = result.Errors[0].Description;

        _cacheService.SetTasks(result.Value);
        _existingTasks = result.Value;
    }

    public async Task Validate()
    {
        var result = await _validator.ValidateAsync(this);

        if (!result.IsValid)
        {
            ErrorMessage = result.Errors[0].ErrorMessage;

            if (result.Errors[0].ErrorCode == "NamePropertyError") NameIsValid = false;
            if (result.Errors[0].ErrorCode == "BeginDatePropertyError") DateIsValid = false;

            return;
        }

        NameIsValid = true;
        DateIsValid = true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}