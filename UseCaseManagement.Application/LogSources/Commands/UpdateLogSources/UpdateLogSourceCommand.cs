using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSources.Commands.UpdateLogSources;

public record UpdateLogSourceCommand(Guid LogSourceId, string Name, string Description, List<Guid> UseCases) : IRequest<ErrorOr<LogSource>>;

public class UpdateLogSourceCommandHandler(ILogSourceRepository sourceRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateLogSourceCommand, ErrorOr<LogSource>>
{
    public async Task<ErrorOr<LogSource>> Handle(UpdateLogSourceCommand request, CancellationToken cancellationToken)
    {
        var sourceEntity = await sourceRepository.GetLogSourceById(request.LogSourceId, cancellationToken);

        if (sourceEntity == null)
        {
            return Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found.");
        }

        sourceEntity.Name = request.Name;
        sourceEntity.Description = request.Description;

        var logSourceUpdated = await sourceRepository.UpdateLogSource(sourceEntity, request.UseCases, cancellationToken);

        if (logSourceUpdated.IsError)
        {
            return logSourceUpdated.Errors;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return logSourceUpdated;
    }
}
