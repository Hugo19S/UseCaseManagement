using FluentValidation;

namespace UseCaseManagement.Application.UseCases.Commands.UpdateUseCases;

public class UpdateUseCaseCommandValidator : AbstractValidator<UpdateUseCaseCommand>
{
    public UpdateUseCaseCommandValidator()
    {
        RuleFor(x => x.UseCaseId)
            .NotEmpty().WithMessage("Enter the UseCaseId to be updated");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("The Title can't be empty.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("The Type can't be empty.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("The Status can't be empty.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("The Category can't be empty.");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("The Priority can't be empty.");
    }
}
