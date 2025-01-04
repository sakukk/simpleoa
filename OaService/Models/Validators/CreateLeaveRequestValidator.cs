using FluentValidation;

namespace OaService.Models.Validators;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequest>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(x => x.ApplicationType)
            .IsInEnum()
            .WithMessage("Invalid application type");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(BeAFutureDate)
            .WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must(BeAFutureDate)
            .WithMessage("End date must be in the future")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be after or equal to start date");

        RuleFor(x => x.ApplicationReason)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(500)
            .WithMessage("Reason must be between 5 and 500 characters");
    }

    private bool BeAFutureDate(DateTime date)
    {
        return date.Date >= DateTime.UtcNow.Date;
    }
} 