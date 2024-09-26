using FluentValidation;

namespace UseCaseManagement.Application.Tenants.Commands.DeleteTenants;

public class DeleteTenantCommandValidator : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("The TenantId can't be empty.");
    }
}
