using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using StudyTrackerUi.Validations;

namespace StudyTrackerUi.Views;

public sealed class CreateTaskViewModel : INotifyPropertyChanged
{
    private readonly TaskValidator _taskValidator = new();
    private DateOnly _beginDate;
    private string _description;
    private DateOnly _endDate;
    private string _name;
    private string _nameErrorMessage;
    private bool _nameIsValid;
    private bool _success;

    public CreateTaskViewModel()
    {
        //  without this, even if the user did not have time to enter anything, entry will be highlighted with an error
        _nameIsValid = true;
        ValidateNameCommand = new RelayCommand(Validate);
    }

    public IRelayCommand ValidateNameCommand { get; private set; }

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

    public DateOnly BeginDate
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

    public DateOnly EndDate
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

    public string NameErrorMessage
    {
        get => _nameErrorMessage;
        set
        {
            if (_nameErrorMessage != value)
            {
                _nameErrorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Validate()
    {
        var result = _taskValidator.Validate(this);
        if (!result.IsValid)
        {
            NameErrorMessage = result.Errors
                .FirstOrDefault(e => e.PropertyName == nameof(Name))
                ?.ErrorMessage!;
            NameIsValid = false;
            return;
        }

        NameIsValid = true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}