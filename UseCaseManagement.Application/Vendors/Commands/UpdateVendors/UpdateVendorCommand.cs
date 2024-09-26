using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Vendors.Commands.UpdateVendors;

public record UpdateVendorCommand(Guid VendorId, string Name) : IRequest<ErrorOr<Vendor>>;

public class UpdateVendorCommandHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateVendorCommand, ErrorOr<Vendor>>
{
    public async Task<ErrorOr<Vendor>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendorExist = await vendorRepository.GetVendorById(request.VendorId, cancellationToken);

        if (vendorExist == null)
        {
            return Error.NotFound("Vendor.NotFound", $"Vendor with id {request.VendorId} not found.");
        }

        vendorExist.Name = request.Name;

        var updated = await vendorRepository.UpdateVendor(vendorExist, cancellationToken);

        if (updated == null)
        {
            return Error.Conflict("Product.Conflict", "There is already a vendor with the same name.");
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return updated;
    }
}
