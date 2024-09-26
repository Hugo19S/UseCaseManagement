using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.UseCaseFiles.Commands.DeleteUseCaseFiles;

public record DeleteUseCaseFileCommand(Guid FileId) : IRequest<ErrorOr<Guid>>;

public class DeleteUseCaseFileCommandHandler(IUseCaseFileRepository fileRepository,
                                               IUnitOfWork unitOfWork) : IRequestHandler<DeleteUseCaseFileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteUseCaseFileCommand request, CancellationToken cancellationToken)
    {
        var useCaseFile = await fileRepository.GetUseCaseFileById(request.FileId, cancellationToken);

        if (useCaseFile == null)
        {
            return Error.NotFound("File.NotFound", $"File with id {request.FileId} not found.");
        }

        await fileRepository.DeleteUseCaseFile(request.FileId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        fileRepository.DeleteFile(useCaseFile.Uri);

        return request.FileId;
    }
}
