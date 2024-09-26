using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Vendors.Commands.CreateVendors;

public record CreateVendorCommand(string VendorName) : IRequest<ErrorOr<Vendor>>;

public class CreateVendorCommandHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateVendorCommand, ErrorOr<Vendor>>
{
    public async Task<ErrorOr<Vendor>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var createdVendor = new Vendor
        {
            Id = Guid.NewGuid(),
            Name = request.VendorName
        };

        var vendorAddedToDb = await vendorRepository.AddVendor(createdVendor, cancellationToken);

        if (vendorAddedToDb.IsError)
        {
            return vendorAddedToDb.Errors;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return createdVendor;
    }
}
