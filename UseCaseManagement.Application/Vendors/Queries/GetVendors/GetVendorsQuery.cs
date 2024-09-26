using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Vendors.Queries.GetVendors;

public record GetVendorsQuery() : IRequest<ErrorOr<List<Vendor>>>;

public class GetVendorsQueryHandler(IVendorRepository vendorRepository) : IRequestHandler<GetVendorsQuery, ErrorOr<List<Vendor>>>
{
    public async Task<ErrorOr<List<Vendor>>> Handle(GetVendorsQuery request, CancellationToken cancellationToken)
    {
        return await vendorRepository.GetAllVendor(cancellationToken);
    }
}
