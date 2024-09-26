using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.Tenants.Commands.AddTenantToUseCase;

public record AddTenantToUseCaseCommand(Guid UseCaseId, Guid TenantId) : IRequest<ErrorOr<UseCase>>;

public class AddTenantToUseCaseCommandHandler(IUseCaseRepository caseRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddTenantToUseCaseCommand, ErrorOr<UseCase>>
{
    public async Task<ErrorOr<UseCase>> Handle(AddTenantToUseCaseCommand request, CancellationToken cancellationToken)
    {
        UseCase? useCase = await caseRepository.GetUseCaseById(request.UseCaseId, cancellationToken);

        if (useCase == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found.");
        }

        if (useCase.Tenants.Contains(request.TenantId))
        {
            return Error.Conflict("Tenants.Conflit", $"UseCase with id {request.UseCaseId} contain a tenant with id {request.TenantId}.");
        }

        caseRepository.AddTenantToUseCase(useCase, request.TenantId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return useCase;
    }
}
