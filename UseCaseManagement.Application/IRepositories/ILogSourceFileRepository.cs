using ErrorOr;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.IRepositories;

public interface ILogSourceFileRepository : IFileOperation
{
    Task<ErrorOr<Created>> AddLogSourceFile(LogSourceFile logSourceFile, CancellationToken cancellationToken);
    Task<LogSourceFile?> GetLogSourceFileById(Guid logSourceFileId, CancellationToken cancellationToken);
    Task DeleteLogSourceFile(Guid fileId, CancellationToken cancellationToken);
}
