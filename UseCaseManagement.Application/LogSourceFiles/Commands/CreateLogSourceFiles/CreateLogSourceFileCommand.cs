using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSourceFiles.Commands.CreateLogSourceFiles;

public record CreateLogSourceFileCommand(Guid LogSourceId, FileRepresentation? File) : IRequest<ErrorOr<LogSourceFile>>;

public class CreateLogSourceFileCommandHandler(ILogSourceFileRepository fileRepository,
                                               ILogSourceRepository logSourceRepository,
                                               IUnitOfWork unitOfWork) : IRequestHandler<CreateLogSourceFileCommand, ErrorOr<LogSourceFile>>
{
    public async Task<ErrorOr<LogSourceFile>> Handle(CreateLogSourceFileCommand request, CancellationToken cancellationToken)
    {
        var logSource = await logSourceRepository.GetLogSourceById(request.LogSourceId, cancellationToken);

        if (logSource == null)
        {
            return Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found.");
        }

        Guid fileId = Guid.NewGuid();

        string filePath = await fileRepository.SaveFile(fileId, request.LogSourceId, request.File!, cancellationToken);

        var createFile = new LogSourceFile
        {
            Id = fileId,
            LogSourceId = request.LogSourceId,
            FileName = request.File!.Name,
            FileSize = (int)request.File.Size,
            Uri = filePath,
            Type = request.File.ContentType
        };

        try
        {
            await fileRepository.AddLogSourceFile(createFile, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return createFile;
        }
        catch (DbUpdateException)
        {
            fileRepository.DeleteFile(filePath);
            return Error.Failure("LogSource.Failure", "A failure occurred while saving the data.");
        } 
    }
}
