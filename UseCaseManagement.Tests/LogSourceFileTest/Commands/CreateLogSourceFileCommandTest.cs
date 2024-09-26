using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSourceFiles.Commands.CreateLogSourceFiles;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceFileTest.Commands;

public class CreateLogSourceFileCommandTest
{
    [Fact]
    public async void CreateLogSourceFileCommand_Return_LogSource_NotFound()
    {
        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((LogSource?)null);
        
        var mockLogSourceFileRepository = new Mock<ILogSourceFileRepository>();
        mockLogSourceFileRepository.Setup(x => x.SaveFile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<FileRepresentation>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        mockLogSourceFileRepository.Setup(x => x.AddLogSourceFile(It.IsAny<LogSourceFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateLogSourceFileCommandHandler handler = new(mockLogSourceFileRepository.Object, mockLogSourceRepository.Object, mockUnitOfWork.Object);

        FileRepresentation? fileRepresentation = null;
        CreateLogSourceFileCommand request = new(Guid.NewGuid(), fileRepresentation);

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(logSourceFileResult.IsError);
        Assert.Equal(Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found."), logSourceFileResult.FirstError);
    }

    [Fact]
    public async void CreateLogSourceFileCommand_Return_Created()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };
        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);

        var mockLogSourceFileRepository = new Mock<ILogSourceFileRepository>();
        mockLogSourceFileRepository.Setup(x => x.SaveFile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<FileRepresentation>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        mockLogSourceFileRepository.Setup(x => x.AddLogSourceFile(It.IsAny<LogSourceFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateLogSourceFileCommandHandler handler = new(mockLogSourceFileRepository.Object, mockLogSourceRepository.Object, mockUnitOfWork.Object);

        FileRepresentation fileRepresentation = new()
        {
            Name = "Comprovativo de matricula.pdf",
            ContentType = "application/pdf",
            Size = 75686,
            Content = new FileStream(@"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\LogSourcesFiles\22182d29-0613-4eb6-8a75-4ae8e625b872\68d5290a-5b1c-4510-a65a-886e0e5fd927.pdf", FileMode.Open)
        };
        CreateLogSourceFileCommand request = new(Guid.NewGuid(), fileRepresentation);

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(logSourceFileResult.IsError);
        Assert.Equal(request.File!.Name, logSourceFileResult.Value.FileName);
    }

}
