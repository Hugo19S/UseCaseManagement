using FluentValidation;

namespace UseCaseManagement.Application.LogSources.Commands.CreateLogSources;

public class CreateLogSourceCommandValidator : AbstractValidator<CreateLogSourceCommand>
{
    public CreateLogSourceCommandValidator()
    {
        RuleFor(x =>  x.Name)
            .NotEmpty().WithMessage("The Name can't be empty.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The Description can't be empty.");
    }
}
