using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.UseCaseFiles.Commands.CreateUseCaseFiles;

public record CreateUseCaseFileCommand(Guid UseCaseId, FileRepresentation? File) : IRequest<ErrorOr<UseCaseFile>>;

public class CreateUseCaseFileCommandHandler(IUseCaseFileRepository fileRepository,
                                               IUseCaseRepository useCaseRepository,
                                               IUnitOfWork unitOfWork) : IRequestHandler<CreateUseCaseFileCommand, ErrorOr<UseCaseFile>>
{
    public async Task<ErrorOr<UseCaseFile>> Handle(CreateUseCaseFileCommand request, CancellationToken cancellationToken)
    {
        var useCase = await useCaseRepository.GetUseCaseById(request.UseCaseId, cancellationToken);

        if (useCase == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found.");
        }

        Guid fileId = Guid.NewGuid();

        string filePath = await fileRepository.SaveFile(fileId, request.UseCaseId, request.File!, cancellationToken);

        var createFile = new UseCaseFile
        {
            Id = fileId,
            UseCaseId = request.UseCaseId,
            FileName = request.File!.Name,
            FileSize = (int)request.File.Size,
            Uri = filePath,
            Type = request.File.ContentType
        };

        try
        {
            await fileRepository.AddUseCaseFile(createFile, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            fileRepository.DeleteFile(filePath);
        }

        return createFile;
    }
}
