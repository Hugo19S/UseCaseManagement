using FluentValidation;

namespace UseCaseManagement.Application.Tenants.Queries.GetTenant;

public class GetTenantQueryValidation : AbstractValidator<GetTenantQuery>
{
    public GetTenantQueryValidation()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("The TenantId can't be empty.");
    }
}
