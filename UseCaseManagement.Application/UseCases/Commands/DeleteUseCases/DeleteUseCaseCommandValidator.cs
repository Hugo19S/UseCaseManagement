using FluentValidation;

namespace UseCaseManagement.Application.UseCases.Commands.DeleteUseCases;

public class DeleteUseCaseCommandValidator : AbstractValidator<DeleteUseCaseCommand>
{
    public DeleteUseCaseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The Id can't be empty.");
    }
}
