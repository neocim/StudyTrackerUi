using FluentValidation;
using StudyTrackerUi.ViewModels;

namespace StudyTrackerUi.Validations;

public class TaskValidator : AbstractValidator<CreateTaskViewModel>
{
    public TaskValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .WithErrorCode("NamePropertyError");

        RuleFor(t => t.BeginDate).LessThan(t => t.EndDate)
            .WithMessage("Start date must be less than end date")
            .WithErrorCode("BeginDatePropertyError");
    }
}