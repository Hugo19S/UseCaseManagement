using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Vendors.Queries.GetVendor;

public record GetVendorQuery(Guid VendorId) : IRequest<ErrorOr<Vendor>>;

public class GetVendorQueryHandler(IVendorRepository vendorRepository) : IRequestHandler<GetVendorQuery, ErrorOr<Vendor>>
{
    public async Task<ErrorOr<Vendor>> Handle(GetVendorQuery request, CancellationToken cancellationToken)
    {
        var result = await vendorRepository.GetVendorById(request.VendorId, cancellationToken);

        if (result == null)
        {
            return Error.NotFound("Vendor.NotFound", $"Vendor with id {request.VendorId} not found.");
        }

        return result;
    }
}