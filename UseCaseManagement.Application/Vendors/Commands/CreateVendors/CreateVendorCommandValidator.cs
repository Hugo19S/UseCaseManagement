using FluentValidation;

namespace UseCaseManagement.Application.Vendors.Commands.CreateVendors;

public class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorCommandValidator()
    {
        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("The VendorName can't be empty.");
    }
}
