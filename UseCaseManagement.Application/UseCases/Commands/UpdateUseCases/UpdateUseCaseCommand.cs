using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.UseCases.Commands.UpdateUseCases;

public record UpdateUseCaseCommand(Guid UseCaseId, string Title, string Type, string Status,
                                   string Category, string Tag, string Priority,
                                   List<string> MitreAttacks, List<Guid> Tenants, string UpdatedBy,
                                   List<Guid> LogSources, List<Guid> Vendors) : IRequest<ErrorOr<UseCase>>;

public class UpdateUseCaseCommandHandler(IUseCaseRepository useCaseRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUseCaseCommand, ErrorOr<UseCase>>
{
    public async Task<ErrorOr<UseCase>> Handle(UpdateUseCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await useCaseRepository.GetUseCaseById(request.UseCaseId, cancellationToken);

        if (caseEntity == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found.");
        }

        request.Tenants.AddRange(caseEntity.Tenants);

        caseEntity.Tenants = request.Tenants.Distinct().ToList();

        caseEntity.Title = request.Title;
        caseEntity.Type = request.Type;
        caseEntity.Status = request.Status;
        caseEntity.Category = request.Category;
        caseEntity.Tag = request.Tag;
        caseEntity.Priority = request.Priority;
        caseEntity.MitreAttacks = request.MitreAttacks;
        caseEntity.UpdatedBy = request.UpdatedBy;
        caseEntity.UpdatedAt = DateTimeOffset.UtcNow;
        
        var useCaseUpdated = await useCaseRepository.UpdateUseCase(caseEntity, request.LogSources, request.Vendors, cancellationToken);

        if (useCaseUpdated.IsError)
        {
            return useCaseUpdated.Errors;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return useCaseUpdated;
    }
}
