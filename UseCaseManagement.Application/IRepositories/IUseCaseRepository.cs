using ErrorOr;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Domain.Filters;

namespace UseCaseManagement.Application.IRepositories;

public interface IUseCaseRepository
{
    Task<UseCase?> AddUseCase(UseCase useCase, CancellationToken cancellationToken);
    Task<List<UseCase>> GetAllUseCase(UseCaseFilter useCaseFilter, CancellationToken cancellationToken);
    Task<UseCase?> GetUseCaseById(Guid id, CancellationToken cancellationToken);
    Task DeleteUseCase(Guid useCaseId, CancellationToken cancellationToken);
    Task<ErrorOr<UseCase>> UpdateUseCase(UseCase useCase, List<Guid> LogSources, List<Guid> Vendors, CancellationToken cancellationToken);
    Task<List<UseCase>> GetTenant(Guid tenantId, CancellationToken cancellationToken);
    void AddTenantToUseCase(UseCase useCase, Guid tenantId, CancellationToken cancellationToken);
    Task DeleteTenant(List<UseCase> useCaseList, Guid tenantId, CancellationToken cancellationToken);
    void DeleteTenantFormUseCase(UseCase useCase, Guid tenantId, CancellationToken cancellationToken);
}
