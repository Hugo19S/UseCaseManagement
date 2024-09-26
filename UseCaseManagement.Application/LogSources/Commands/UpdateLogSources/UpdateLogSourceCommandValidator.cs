using FluentValidation;
using UseCaseManagement.Application.LogSources.Commands.UpdateLogSources;

namespace UseCaseManagement.Application.LogSources.Commands.UpdateLogSources;

public class UpdateLogSourceCommandValidator : AbstractValidator<UpdateLogSourceCommand>
{
    public UpdateLogSourceCommandValidator()
    {
        RuleFor(x => x.LogSourceId)
            .NotEmpty().WithMessage("Enter the LogSourceId to be updated.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name can't be empty.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The Description can't be empty.");
    }
}
