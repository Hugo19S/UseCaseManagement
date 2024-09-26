using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSources.Queries.GetLogSources;

public record GetLogSourcesQuery() : IRequest<ErrorOr<List<LogSource>>>;

public class GetLogSourcesQueryHandler(ILogSourceRepository sourceRepository) : IRequestHandler<GetLogSourcesQuery, ErrorOr<List<LogSource>>>
{
    public async Task<ErrorOr<List<LogSource>>> Handle(GetLogSourcesQuery request, CancellationToken cancellationToken)
    {
        return await sourceRepository.GetAllLogSource(cancellationToken);
    }
}
