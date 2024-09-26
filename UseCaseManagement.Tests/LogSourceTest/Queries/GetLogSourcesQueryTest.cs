using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Queries.GetLogSources;
using UseCaseManagement.Application.UseCases.Queries.GetUseCases;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Domain.Filters;

namespace UseCaseManagement.Tests.LogSourceTest.Queries;

public class GetLogSourcesQueryTest
{
    [Fact]
    public async void GetLogSourcesQuery_Return_LogSourceList()
    {
        List<LogSource> logSources =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
            }
        ];
        var mockRepository = new Mock<ILogSourceRepository>();
        mockRepository.Setup(x => x.GetAllLogSource(It.IsAny<CancellationToken>())).ReturnsAsync(logSources);

        GetLogSourcesQueryHandler handler = new(mockRepository.Object);
        GetLogSourcesQuery request = new();

        var logSourceResult = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(logSourceResult.Value);
        Assert.IsType<List<LogSource>>(logSourceResult.Value);
        Assert.Equal(logSources, logSourceResult.Value);
    }

    [Fact]
    public async void GetLogSourcesQuery_Return_Empty()
    {
        var mockRepository = new Mock<ILogSourceRepository>();
        mockRepository.Setup(x => x.GetAllLogSource(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        GetLogSourcesQueryHandler handler = new(mockRepository.Object);
        GetLogSourcesQuery request = new();

        var logSourceResult = await handler.Handle(request, CancellationToken.None);

        Assert.Empty(logSourceResult.Value);
        Assert.IsType<List<LogSource>>(logSourceResult.Value);
    }
}
