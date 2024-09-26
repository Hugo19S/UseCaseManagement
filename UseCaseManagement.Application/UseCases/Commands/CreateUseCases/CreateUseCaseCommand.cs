using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.UseCases.Commands.CreateUseCases;

public record CreateUseCaseCommand(string Title, string Type, string Status, string Category, string Tag, 
                                   string Priority, List<string> MitreAttacks, List<Guid> Tenants, string CreatedBy) : IRequest<ErrorOr<UseCase>>;

public class CreateUseCaseCommandHandler(IUseCaseRepository useCaseRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUseCaseCommand, ErrorOr<UseCase>>
{
    public async Task<ErrorOr<UseCase>> Handle(CreateUseCaseCommand request, CancellationToken cancellationToken)
    {
        var useCaseCreated = new UseCase 
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Type = request.Type,
            Status = request.Status,
            Category = request.Category,
            Tag = request.Tag,
            Priority = request.Priority,
            MitreAttacks = request.MitreAttacks,
            Tenants = request.Tenants.Distinct().ToList(),
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var useCaseStored = await useCaseRepository.AddUseCase(useCaseCreated, cancellationToken);

        if (useCaseStored == null)
        {
            return Error.Conflict("UseCase.Conflict", "There is already a UseCase with the same Title.");
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return useCaseStored;
    }
}
