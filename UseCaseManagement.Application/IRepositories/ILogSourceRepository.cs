using ErrorOr;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.IRepositories;

public interface ILogSourceRepository
{
    Task<ErrorOr<Created>> AddLogSource(LogSource logSource, CancellationToken cancellationToken);
    Task<List<LogSource>> GetAllLogSource(CancellationToken cancellationToken);
    Task<LogSource?> GetLogSourceById(Guid id, CancellationToken cancellationToken);
    Task DeleteLogSource(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<LogSource>> UpdateLogSource(LogSource logSource, List<Guid> useCasesIds, CancellationToken cancellationToken);
}
