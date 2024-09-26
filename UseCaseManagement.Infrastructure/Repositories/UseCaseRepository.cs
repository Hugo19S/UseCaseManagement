using ErrorOr;
using Microsoft.EntityFrameworkCore;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Domain.Filters;

namespace UseCaseManagement.Infrastructure.Repositories;

public class UseCaseRepository(UseCaseDbContext dbContext) : IUseCaseRepository
{
    public async Task<UseCase?> AddUseCase(UseCase useCase, CancellationToken cancellationToken)
    {
        var useCaseExist = await dbContext.UseCase.FirstOrDefaultAsync(x => x.Title == useCase.Title, cancellationToken);

        if (useCaseExist == null)
        {
            var useCaseCreated = await dbContext.UseCase.AddAsync(useCase, cancellationToken);
            return useCaseCreated.Entity;
        }

        return null;
    }

    public async Task DeleteUseCase(Guid useCaseId, CancellationToken cancellationToken)
    {
        await dbContext.UseCase.Where(x => x.Id == useCaseId).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<UseCase>> GetAllUseCase(UseCaseFilter useCaseFilter, CancellationToken cancellationToken)
    {
        var query = dbContext.UseCase.AsQueryable();

        if (!string.IsNullOrEmpty(useCaseFilter.Type)) query = query.Where(x => x.Type == useCaseFilter.Type);
        
        if (!string.IsNullOrEmpty(useCaseFilter.Status)) query = query.Where(x => x.Status == useCaseFilter.Status);

        if (!string.IsNullOrEmpty(useCaseFilter.Category)) query = query.Where(x => x.Category == useCaseFilter.Category);
        
        if (!string.IsNullOrEmpty(useCaseFilter.Priority)) query = query.Where(x => x.Priority == useCaseFilter.Priority);
        
        if (!string.IsNullOrEmpty(useCaseFilter.Tag)) query = query.Where(x => x.Tag == useCaseFilter.Tag);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<UseCase?> GetUseCaseById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.UseCase
            .Include(x => x.Files)
            .Include(x => x.LogSources)
            .Include(x => x.Vendors)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ErrorOr<UseCase>> UpdateUseCase(UseCase useCase, List<Guid> logSourcesIds, List<Guid> vendorsIds, CancellationToken cancellationToken)
    {
        var sameTitle = await dbContext.UseCase.FirstOrDefaultAsync(x => x.Title == useCase.Title && x.Id != useCase.Id, cancellationToken);

        if (sameTitle != null)
        {
            return Error.Conflict("UseCase.Conflict", "There is already a UseCase with the same Title.");
        }

        try
        {
            List<LogSource> sourceList = [];
            foreach (var id in logSourcesIds)
            {
                var logSource = await dbContext.LogSource.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (logSource == null)
                {
                    return Error.NotFound("LogSource.NotFound", $"LogSource with id {id} not found.");
                }

                sourceList.Add(logSource);
            }

            useCase.LogSources = sourceList.Distinct().ToList();
        }
        catch (NullReferenceException)
        {
            useCase.LogSources = [];
        }

        try
        {
            List<Vendor> vendorList = [];
            foreach (var id in vendorsIds)
            {
                var vendor = await dbContext.Vendor.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (vendor == null)
                {
                    return Error.NotFound("Vendor.NotFound", $"Vendor with id {id} not found.");
                }

                vendorList.Add(vendor);
            }

            useCase.Vendors = vendorList.Distinct().ToList();
        }
        catch (NullReferenceException)
        {
            useCase.Vendors = [];
        }

        return useCase;
    }

    public void AddTenantToUseCase(UseCase useCase, Guid tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        useCase.Tenants.Add(tenantId);
    }

    public async Task<List<UseCase>> GetTenant(Guid tenantId, CancellationToken cancellationToken)
    {
        return await dbContext.UseCase.Where(x => x.Tenants.Contains(tenantId)).ToListAsync(cancellationToken);
    }

    public Task DeleteTenant(List<UseCase> useCaseList, Guid tenantId, CancellationToken cancellationToken)
    {
        foreach (var useCase in useCaseList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            useCase.Tenants.Remove(tenantId);
        }
        return Task.CompletedTask;
    }

    public void DeleteTenantFormUseCase(UseCase useCase, Guid tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        useCase.Tenants.Remove(tenantId);
    }
}
