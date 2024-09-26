using UseCaseManagement.Domain.Common;

namespace UseCaseManagement.Application.Common;

public interface IFileOperation
{
    Task<string> SaveFile(Guid fileId, Guid useCaseId, FileRepresentation file, CancellationToken cancellationToken);
    Task<FileStream> ReadFile(string uri, CancellationToken cancellationToken);
    void DeleteFile(string filePath);
}
