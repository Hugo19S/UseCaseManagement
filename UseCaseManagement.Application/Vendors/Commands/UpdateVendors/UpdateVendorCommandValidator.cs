using FluentValidation;

namespace UseCaseManagement.Application.Vendors.Commands.UpdateVendors;

public class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Enter the id of the vendor to be updated."); ;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The Name can't be empty."); ;
    }
}
