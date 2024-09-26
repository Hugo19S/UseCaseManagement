using FluentValidation;

namespace UseCaseManagement.Application.Vendors.Queries.GetVendor;

public class GetVendorQueryValidator : AbstractValidator<GetVendorQuery>
{
    public GetVendorQueryValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("The VendorId can't be empty.");
    }
}
