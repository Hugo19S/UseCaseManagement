using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.LogSources.Commands.DeleteLogSources;

public record DeleteLogSourceCommand(Guid LogSourceId) : IRequest<ErrorOr<Guid>>;

public class DeleteLogSourceCommandHandler(ILogSourceRepository sourceRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteLogSourceCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteLogSourceCommand request, CancellationToken cancellationToken)
    {
        var logSourceExist = await sourceRepository.GetLogSourceById(request.LogSourceId, cancellationToken);

        if (logSourceExist == null)
        {
            return Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found.");
        }

        await sourceRepository.DeleteLogSource(request.LogSourceId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return request.LogSourceId;
    }
}
