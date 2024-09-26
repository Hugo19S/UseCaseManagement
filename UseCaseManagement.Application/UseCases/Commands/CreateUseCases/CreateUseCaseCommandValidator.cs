using FluentValidation;

namespace UseCaseManagement.Application.UseCases.Commands.CreateUseCases;

public class CreateUseCaseCommandValidator : AbstractValidator<CreateUseCaseCommand>
{
    public CreateUseCaseCommandValidator()
    {
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
    
        RuleFor(x => x.MitreAttacks)
            .NotEmpty().WithMessage("The MitreAttacks can't be empty.");
        
        RuleFor(x => x.Tenants)
            .NotEmpty().WithMessage("The Tenants can't be empty.");
    }
}
