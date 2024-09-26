using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSourceFiles.Commands.DeleteLogSourceFiles;
using UseCaseManagement.Application.UseCaseFiles.Commands.DeleteUseCaseFiles;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseFileTest.Commands;

public class DeleteUseCaseFileCommandTest
{
    [Fact]
    public async void DeleteUseCaseFileCommand_Return_File_NotFound()
    {
        var mockUseCaseFileRepository = new Mock<IUseCaseFileRepository>();
        mockUseCaseFileRepository.Setup(x => x.GetUseCaseFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCaseFile?)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteUseCaseFileCommandHandler handler = new(mockUseCaseFileRepository.Object, mockUnitOfWork.Object);
        DeleteUseCaseFileCommand request = new(Guid.NewGuid());

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(useCaseFileResult.IsError);
        Assert.Equal(Error.NotFound("File.NotFound", $"File with id {request.FileId} not found."), useCaseFileResult.FirstError);
    }

    [Fact]
    public async void DeleteUseCaseFileCommand_Return_Deleted()
    {
        UseCaseFile file = new()
        {
            Id = Guid.Parse("0783d097-61af-4744-8277-06d6473ea413"),
            FileName = "Comprovativo de matricula.pdf",
            FileSize = 75686,
            Uri = @"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\UseCaseFiles\2cf772a4-4a42-4b0d-9dea-cd7631563166\0783d097-61af-4744-8277-06d6473ea413.pdf",
            Type = "application/pdf"
        };

        var mockUseCaseFileRepository = new Mock<IUseCaseFileRepository>();
        mockUseCaseFileRepository.Setup(x => x.GetUseCaseFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(file);
        mockUseCaseFileRepository.Setup(x => x.DeleteUseCaseFile(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockUseCaseFileRepository.Setup(x => x.DeleteFile(It.IsAny<string>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteUseCaseFileCommandHandler handler = new(mockUseCaseFileRepository.Object, mockUnitOfWork.Object);
        DeleteUseCaseFileCommand request = new(file.Id);

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseFileResult.IsError);
        Assert.Equal(file.Id, useCaseFileResult.Value);
    }

}
