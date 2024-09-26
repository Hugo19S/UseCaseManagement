using FluentValidation;

namespace UseCaseManagement.Application.Tenants.Commands.DeleteTenantFromUseCase;

public class DeleteTenantUseCaseCommandValidator : AbstractValidator<DeleteTenantUseCaseCommand>
{
    public DeleteTenantUseCaseCommandValidator()
    {
        RuleFor(x => x.UseCaseId)
            .NotEmpty().WithMessage("The VendorId can't be empty.");

        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("The TenantId can't be empty.");
    }
}
