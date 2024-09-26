using ErrorOr;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.IRepositories;

public interface IUseCaseFileRepository : IFileOperation
{
    Task<ErrorOr<Created>> AddUseCaseFile(UseCaseFile file, CancellationToken cancellationToken);
    Task<UseCaseFile?> GetUseCaseFileById(Guid id, CancellationToken cancellationToken);
    Task DeleteUseCaseFile(Guid fileId, CancellationToken cancellationToken);
}
