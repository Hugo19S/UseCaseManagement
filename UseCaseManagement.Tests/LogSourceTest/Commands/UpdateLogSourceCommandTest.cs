using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Commands.UpdateLogSources;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceTest.Commands;

public class UpdateLogSourceCommandTest
{
    [Fact]
    public async void UpdateLogSourceQuery_Return_LogSource_NotFound()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };

        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((LogSource?)null);
        mockLogSourceRepository.Setup(x => x.UpdateLogSource(It.IsAny<LogSource>(), It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        

        UpdateLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);

        UpdateLogSourceCommand request = new(logSource.Id, "New test", "New description", []);

        var resultLogSourceUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultLogSourceUpdated.IsError);
        Assert.Equal(Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found."), resultLogSourceUpdated.FirstError);
    }

    [Fact]
    public async void UpdateLogSourceQuery_Return_LogSource_Conflit()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };

        Error error = Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Title.");

        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);
        mockLogSourceRepository.Setup(x => x.UpdateLogSource(It.IsAny<LogSource>(), It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(error);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);

        UpdateLogSourceCommand request = new(logSource.Id, "New test", "New description", []);

        var resultLogSourceUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultLogSourceUpdated.IsError);
        Assert.Equal(Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Title."), resultLogSourceUpdated.FirstError);
    }

    [Fact]
    public async void UpdateLogSourceQuery_Return_LogSource()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };

        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);
        mockLogSourceRepository.Setup(x => x.UpdateLogSource(It.IsAny<LogSource>(), It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);

        UpdateLogSourceCommand request = new(logSource.Id, "New test", "New description", []);

        var resultLogSourceUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.False(resultLogSourceUpdated.IsError);
        Assert.Equal(logSource.Id, resultLogSourceUpdated.Value.Id);
        Assert.Equal(logSource, resultLogSourceUpdated.Value);
    }
}
