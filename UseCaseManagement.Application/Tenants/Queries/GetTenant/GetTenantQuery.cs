using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Tenants.Queries.GetTenant;

public record GetTenantQuery(Guid TenantId) : IRequest<ErrorOr<List<UseCase>>>;

public class GetTenantQueryHandler(IUseCaseRepository caseRepository) : IRequestHandler<GetTenantQuery, ErrorOr<List<UseCase>>>
{
    public async Task<ErrorOr<List<UseCase>>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        return await caseRepository.GetTenant(request.TenantId, cancellationToken);
    }
}
