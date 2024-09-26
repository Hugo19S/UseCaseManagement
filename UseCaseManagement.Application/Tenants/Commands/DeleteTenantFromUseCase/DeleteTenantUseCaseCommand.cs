using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Tenants.Commands.DeleteTenantFromUseCase;

public record DeleteTenantUseCaseCommand(Guid UseCaseId, Guid TenantId) : IRequest<ErrorOr<UseCase>>;

public class DeleteTenantUseCaseCommandHandler(IUseCaseRepository caseRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTenantUseCaseCommand, ErrorOr<UseCase>>
{
    public async Task<ErrorOr<UseCase>> Handle(DeleteTenantUseCaseCommand request, CancellationToken cancellationToken)
    {
        UseCase? useCase = await caseRepository.GetUseCaseById(request.UseCaseId, cancellationToken);

        if (useCase == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found.");
        }

        if (!useCase.Tenants.Contains(request.TenantId))
        {
            return Error.NotFound("Tenants.NotFound", $"UseCase with id {request.UseCaseId} does't contain a tenant with id {request.TenantId}.");
        }

        caseRepository.DeleteTenantFormUseCase(useCase, request.TenantId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return useCase;
    }
}
