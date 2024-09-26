using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Commands.DeleteLogSources;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceTest.Commands;

public class DeleteLogSourceCommandTest
{
    [Fact]
    public async void DeleteLogSourceCommand_Return_NotFound()
    {
        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.DeleteLogSource(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);
        DeleteLogSourceCommand request = new(Guid.NewGuid());

        var resultLogSourceDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultLogSourceDeleted.IsError);
        Assert.Equal(Error.NotFound("LogSource.NotFound", $"LogSource with id {request.LogSourceId} not found."), resultLogSourceDeleted.FirstError);
    }

    [Fact]
    public async void DeleteLogSourceCommand_Return_DeletedId()
    {
        LogSource logSource = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = "Description"
        };

        var mockLogSourceRepository = new Mock<ILogSourceRepository>();
        mockLogSourceRepository.Setup(x => x.DeleteLogSource(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockLogSourceRepository.Setup(x => x.GetLogSourceById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(logSource);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteLogSourceCommandHandler handler = new(mockLogSourceRepository.Object, mockUnitOfWork.Object);
        DeleteLogSourceCommand request = new(logSource.Id);

        var resultVendorDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.IsType<Guid>(resultVendorDeleted.Value);
        Assert.Equal(logSource.Id, resultVendorDeleted.Value);
    }
}
