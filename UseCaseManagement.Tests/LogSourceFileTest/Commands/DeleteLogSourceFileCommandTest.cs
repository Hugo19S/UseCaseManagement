using ErrorOr;
using Moq;
using System.Drawing;
using System.Net.Mime;
using System.Xml.Linq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSourceFiles.Commands.DeleteLogSourceFiles;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceFileTest.Commands;

public class DeleteLogSourceFileCommandTest
{
    [Fact]
    public async void DeleteLogSourceFileCommand_Return_File_NotFound()
    {
        var mockLogSourceFileRepository = new Mock<ILogSourceFileRepository>();
        mockLogSourceFileRepository.Setup(x => x.GetLogSourceFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((LogSourceFile?) null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteLogSourceFileCommandHandler handler = new(mockLogSourceFileRepository.Object, mockUnitOfWork.Object);
        DeleteLogSourceFileCommand request = new(Guid.NewGuid());

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(logSourceFileResult.IsError);
        Assert.Equal(Error.NotFound("File.NotFound", $"File with id {request.FileId} not found."), logSourceFileResult.FirstError);
    }

    [Fact]
    public async void DeleteLogSourceFileCommand_Return_Deleted()
    {
        LogSourceFile file = new()
        {
            Id = Guid.NewGuid(),
            LogSourceId = Guid.NewGuid(),
            FileName = "Comprovativo de matricula.pdf",
            FileSize = 75686,
            Uri = @"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\LogSourcesFiles\22182d29-0613-4eb6-8a75-4ae8e625b872\68d5290a-5b1c-4510-a65a-886e0e5fd927.pdf",
            Type = "application/pdf"
        };

        var mockLogSourceFileRepository = new Mock<ILogSourceFileRepository>();
        mockLogSourceFileRepository.Setup(x => x.GetLogSourceFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(file);
        mockLogSourceFileRepository.Setup(x => x.DeleteLogSourceFile(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockLogSourceFileRepository.Setup(x => x.DeleteFile(It.IsAny<string>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteLogSourceFileCommandHandler handler = new(mockLogSourceFileRepository.Object, mockUnitOfWork.Object);
        DeleteLogSourceFileCommand request = new(file.Id);

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(logSourceFileResult.IsError);
        Assert.Equal(new Deleted(), logSourceFileResult.Value);
    }
}
