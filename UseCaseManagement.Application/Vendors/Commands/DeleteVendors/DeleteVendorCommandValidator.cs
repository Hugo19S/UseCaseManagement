using FluentValidation;

namespace UseCaseManagement.Application.Vendors.Commands.DeleteVendors;

public class DeleteVendorCommandValidator : AbstractValidator<DeleteVendorCommand>
{
    public DeleteVendorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Enter the id of the vendor to be deleted.");
    }
}
