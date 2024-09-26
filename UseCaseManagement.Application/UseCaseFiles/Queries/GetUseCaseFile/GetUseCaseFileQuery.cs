using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.UseCaseFiles.Queries.GetUseCaseFile;

public record GetUseCaseFileQuery(Guid FileId) : IRequest<ErrorOr<Tuple<FileStream, string, string>>>;

public class GetUseCaseFileQueryHandler(IUseCaseFileRepository fileRepository) : IRequestHandler<GetUseCaseFileQuery, ErrorOr<Tuple<FileStream, string, string>>>
{
    public async Task<ErrorOr<Tuple<FileStream, string, string>>> Handle(GetUseCaseFileQuery request, CancellationToken cancellationToken)
    {
        var fileEntity = await fileRepository.GetUseCaseFileById(request.FileId, cancellationToken);

        if (fileEntity == null)
        {
            return Error.NotFound("File.NotFound", $"File with id {request.FileId} not found.");
        }

        FileStream fs = await fileRepository.ReadFile(fileEntity.Uri, cancellationToken);

        return Tuple.Create(fs, fileEntity.FileName, fileEntity.Type);
    }
}
