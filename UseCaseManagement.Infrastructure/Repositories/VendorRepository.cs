using ErrorOr;
using Microsoft.EntityFrameworkCore;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Repositories;

public class VendorRepository(UseCaseDbContext dbContext) : IVendorRepository
{
    public async Task<ErrorOr<Created>> AddVendor(Vendor vendor, CancellationToken cancellationToken)
    {
        var vendorExist = await dbContext.Vendor.FirstOrDefaultAsync(x => x.Name == vendor.Name, cancellationToken);

        if (vendorExist != null)
        {
            return Error.Conflict("Vendor.Conflict", "There is already a vendor with the same name.");
        }

        await dbContext.Vendor.AddAsync(vendor, cancellationToken);
        return new Created();
    }

    public async Task DeleteVendor(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.Vendor.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<Vendor>> GetAllVendor(CancellationToken cancellationToken)
    {
        return await dbContext.Vendor.ToListAsync(cancellationToken);
    }

    public async Task<Vendor?> GetVendorById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Vendor.Include(x => x.UseCases).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Vendor?> UpdateVendor(Vendor vendor, CancellationToken cancellationToken)
    {
        Vendor? vendorExist = await dbContext.Vendor.FirstOrDefaultAsync(x => x.Name == vendor.Name, cancellationToken);

        if (vendorExist == null)
        {
            var saved = dbContext.Update(vendor);
            return saved.Entity;
        }

        return null;
        //return Error.Conflict("Product.Conflict", "There is already a vendor with the same name.");
    }
}
