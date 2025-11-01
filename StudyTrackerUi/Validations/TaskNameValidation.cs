using FluentValidation;
using StudyTrackerUi.Views;

namespace StudyTrackerUi.Validations;

public class TaskValidator : AbstractValidator<CreateTaskViewModel>
{
    public TaskValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}