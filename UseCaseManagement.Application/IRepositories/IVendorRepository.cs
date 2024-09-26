using ErrorOr;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.IRepositories;

public interface IVendorRepository
{
    Task<ErrorOr<Created>> AddVendor(Vendor vendor, CancellationToken cancellationToken);
    Task<List<Vendor>> GetAllVendor(CancellationToken cancellationToken);
    Task<Vendor?> GetVendorById(Guid id, CancellationToken tcancellationTokenoken);
    Task DeleteVendor(Guid id, CancellationToken cancellationToken);
    Task<Vendor?> UpdateVendor(Vendor vendor, CancellationToken cancellationToken);
}
