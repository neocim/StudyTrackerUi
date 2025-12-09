using FluentValidation;

namespace StudyTrackerUi.ViewModels.Validators;

public class SubTaskValidator : AbstractValidator<CreateSubTaskViewModel>
{
    public SubTaskValidator()
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