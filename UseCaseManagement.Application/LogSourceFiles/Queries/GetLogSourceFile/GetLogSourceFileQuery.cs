using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSourceFiles.Queries.GetLogSourceFile;

public record GetLogSourceFileQuery(Guid FileId) : IRequest<ErrorOr<Tuple<FileStream, string, string>>>;

public class GetLogSourceFileQueryHandler(ILogSourceFileRepository fileRepository) : IRequestHandler<GetLogSourceFileQuery, ErrorOr<Tuple<FileStream, string, string>>>
{
    public async Task<ErrorOr<Tuple<FileStream, string, string>>> Handle(GetLogSourceFileQuery request, CancellationToken cancellationToken)
    {
        LogSourceFile? fileEntity = await fileRepository.GetLogSourceFileById(request.FileId, cancellationToken);

        if (fileEntity == null)
        {
            return Error.NotFound("File.NotFound", $"File with id {request.FileId} not found.");
        }

        FileStream fs = await fileRepository.ReadFile(fileEntity.Uri, cancellationToken);

        return Tuple.Create(fs, fileEntity.FileName, fileEntity.Type);
    }
}
