using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.Vendors.Commands.DeleteVendors;

public record DeleteVendorCommand(Guid Id) : IRequest<ErrorOr<Guid>>;

public class DeleteVendorCommandHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteVendorCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var vendorExist = await vendorRepository.GetVendorById(request.Id, cancellationToken);

        if (vendorExist == null)
        {
            return Error.NotFound("Vendor.NotFound", $"Vendor with id {request.Id} not found.");
        }

        await vendorRepository.DeleteVendor(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
