using ErrorOr;
using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Queries.GetLogSource;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceTest.Queries;

public class GetLogSourceQueryTest
{
    [Fact]
    public async void GetLogSourceQuery_Return_Null()
    {
        var mockRepository = new Mock<ILogSourceRepository>();
        mockRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((LogSource?)null);

        GetLogSourceQueryHandler handler = new(mockRepository.Object);
        GetLogSourceQuery request = new(Guid.NewGuid());

        var logSourcesResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(logSourcesResult.IsError);
        Assert.Equal(Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found."), logSourcesResult.FirstError);
    }

    [Fact]
    public async void GetLogSourceQuery_Return_SingleObject()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };

        var repoMock = new Mock<ILogSourceRepository>();
        repoMock.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);

        GetLogSourceQueryHandler handler = new(repoMock.Object);

        GetLogSourceQuery request = new(Guid.NewGuid());
        var logSourceResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(logSourceResult.IsError);
        Assert.Equal(logSource, logSourceResult.Value);
    }
}
