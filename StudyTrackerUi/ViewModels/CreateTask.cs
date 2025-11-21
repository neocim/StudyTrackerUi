using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using StudyTrackerUi.Validations;

namespace StudyTrackerUi.ViewModels;

public sealed class CreateTaskViewModel : INotifyPropertyChanged
{
    private readonly TaskValidator _taskValidator = new();
    private DateTime _beginDate = DateTime.Now.Date;
    private bool _dateIsValid;
    private string _description = null!;
    private DateTime _endDate = DateTime.Now.Date.AddDays(1);
    private string _errorMessage = null!;
    private string _name = null!;
    private bool _nameIsValid;
    private bool _success;

    public CreateTaskViewModel()
    {
        //  without this, even if the user did not have time to enter anything, entry will be highlighted with an error
        _nameIsValid = true;
        ValidateCommand = new AsyncRelayCommand(Validate);
    }

    public IAsyncRelayCommand ValidateCommand { get; private set; }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                NameIsValid = true;
                OnPropertyChanged();
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }

    public bool Success
    {
        get => _success;
        set
        {
            if (_success != value)
            {
                _success = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime BeginDate
    {
        get => _beginDate;
        set
        {
            if (_beginDate != value)
            {
                _beginDate = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }

    public bool NameIsValid
    {
        get => _nameIsValid;
        set
        {
            if (_nameIsValid != value)
            {
                _nameIsValid = value;
                OnPropertyChanged();
            }
        }
    }

    public bool DateIsValid
    {
        get => _dateIsValid;
        set
        {
            if (_dateIsValid != value)
            {
                _dateIsValid = value;
                OnPropertyChanged();
            }
        }
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task Validate()
    {
        var result = await _taskValidator.ValidateAsync(this);

        if (!result.IsValid)
        {
            if (result.Errors[0].ErrorCode == "NamePropertyError") NameIsValid = false;
            if (result.Errors[0].ErrorCode == "BeginDatePropertyError") DateIsValid = false;

            ErrorMessage = result.Errors[0].ErrorMessage;
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