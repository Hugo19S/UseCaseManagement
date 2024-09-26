using FluentValidation;

namespace UseCaseManagement.Application.LogSources.Commands.DeleteLogSources;

public class DeleteLogSourceCommandValidator : AbstractValidator<DeleteLogSourceCommand>
{
    public DeleteLogSourceCommandValidator()
    {
        RuleFor(x => x.LogSourceId)
            .NotEmpty().WithMessage("Enter the LogSourceId to be deleted.");
    }
}
