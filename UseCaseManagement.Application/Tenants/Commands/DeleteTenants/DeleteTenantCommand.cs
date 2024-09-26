using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.Tenants.Commands.DeleteTenants;

public record DeleteTenantCommand(Guid TenantId) : IRequest<ErrorOr<Guid>>;

public class DeleteTenantCommandHandler(IUseCaseRepository caseRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTenantCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var useCaseListOfTenantId = await caseRepository.GetTenant(request.TenantId, cancellationToken);

        if (useCaseListOfTenantId.Count == 0)
        {
            return Error.NotFound("Tenant.NotFound", $"Tenant with id {request.TenantId} not found.");
        }

        await caseRepository.DeleteTenant(useCaseListOfTenantId, request.TenantId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return request.TenantId;
    }
}
