using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSources.Queries.GetLogSource;

public record GetLogSourceQuery(Guid LogSourceId) : IRequest<ErrorOr<LogSource>>;

public class GetLogSourceQueryHandler(ILogSourceRepository sourceRepository) : IRequestHandler<GetLogSourceQuery, ErrorOr<LogSource>>
{
    public async Task<ErrorOr<LogSource>> Handle(GetLogSourceQuery request, CancellationToken cancellationToken)
    {
        var logSource = await sourceRepository.GetLogSourceById(request.LogSourceId, cancellationToken);

        if (logSource == null)
        {
            return Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found.");
        }

        return logSource;
    }
}
