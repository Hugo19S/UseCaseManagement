using FluentValidation;

namespace UseCaseManagement.Application.Tenants.Commands.AddTenantToUseCase;

public class AddTenantToUseCaseCommandValidator : AbstractValidator<AddTenantToUseCaseCommand>
{
    public AddTenantToUseCaseCommandValidator()
    {
        RuleFor(x => x.UseCaseId)
            .NotEmpty().WithMessage("The VendorId can't be empty.");

        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("The TenantId can't be empty.");
    }
}
