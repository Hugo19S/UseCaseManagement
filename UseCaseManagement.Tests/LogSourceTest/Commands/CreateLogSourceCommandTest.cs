using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Commands.CreateLogSources;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceTest.Commands;

public class CreateLogSourceCommandTest
{
    [Fact]
    public async void CreateLogSourceCommand_Return_Error_LogSource_Conflict()
    {
        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.AddLogSource(It.IsAny<LogSource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Name."));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);
        CreateLogSourceCommand request = new("New LogSource", "New description");

        var logSourceResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(logSourceResult.IsError);
        Assert.Equal(Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Name."), logSourceResult.FirstError);
    }

    [Fact]
    public async void CreateLogSourceCommand_Return_Created()
    {
        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.AddLogSource(It.IsAny<LogSource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);
        CreateLogSourceCommand request = new("New LogSource", "New description");

        var logSourceResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(logSourceResult.IsError);
        Assert.Equal(request.Name, logSourceResult.Value.Name);
        Assert.Equal(request.Description, logSourceResult.Value.Description);
    }
}
