using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.LogSourceFiles.Commands.DeleteLogSourceFiles;

public record DeleteLogSourceFileCommand(Guid FileId) : IRequest<ErrorOr<Deleted>>;

public class DeleteLogSourceFileCommandHandler(ILogSourceFileRepository fileRepository, 
                                               IUnitOfWork unitOfWork) : IRequestHandler<DeleteLogSourceFileCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteLogSourceFileCommand request, CancellationToken cancellationToken)
    {
        var logSourceFile = await fileRepository.GetLogSourceFileById(request.FileId, cancellationToken);

        if (logSourceFile == null)
        {
            return Error.NotFound("File.NotFound", $"File with id {request.FileId} not found.");
        }

        await fileRepository.DeleteLogSourceFile(request.FileId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        fileRepository.DeleteFile(logSourceFile.Uri);

        return new Deleted();
    }
}
