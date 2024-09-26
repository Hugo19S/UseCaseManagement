using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.UseCases.Commands.UpdateUseCases;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseTest.Commands;

public class UpdateUseCaseCommandTest
{
    [Fact]
    public async void UpdateUseCaseQuery_Return_UseCase_NotFound()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


        UpdateUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);

        UpdateUseCaseCommand request = new(Guid.NewGuid(), "New UseCase", "New type", "New Status", "New category", "New Tag", "New priority", [], [], "Mario", [], []);

        var resultUseCaseUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultUseCaseUpdated.IsError);
        Assert.Equal(Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found."), resultUseCaseUpdated.FirstError);
    }

    [Fact]
    public async void UpdateUseCaseQuery_Return_UseCase_Conflit()
    {
        UseCase useCase = new()
        {
            Id = Guid.Parse("e021475f-b454-47b7-8f39-f22bea5f2788"),
            Title = "Privilege Escalation",
            Type = "Vulnerability",
            Status = "Enable",
            Category = "Medium Risk",
            Tag = "Important",
            Priority = "2",
            MitreAttacks = [
                "T1055.001 - Process Injection: Dynamic-link Library Injection",
                "T1068 - Exploitation for Privilege Escalation",
                "T1078.002 - Valid Accounts: Domain Accounts"
            ],
            Tenants = [],
            CreatedBy = "Mario",
            UpdatedBy = null,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };

        Error error = Error.Conflict("UseCase.Conflict", "There is already a UseCase with the same Title.");

        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);
        mockUseCaseRepository.Setup(x => x.UpdateUseCase(It.IsAny<UseCase>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(error);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);

        UpdateUseCaseCommand request = new(Guid.NewGuid(), "New UseCase", "New type", "New Status", "New category", "New Tag", "New priority", [], [], "Mario", [], []);

        var resultUseCaseUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultUseCaseUpdated.IsError);
        Assert.Equal(Error.Conflict("UseCase.Conflict", "There is already a UseCase with the same Title."), resultUseCaseUpdated.FirstError);
    }

    [Fact]
    public async void UpdateUseCaseQuery_Return_UseCase()
    {
        UseCase useCase = new()
        {
            Id = Guid.Parse("e021475f-b454-47b7-8f39-f22bea5f2788"),
            Title = "Privilege Escalation",
            Type = "Vulnerability",
            Status = "Enable",
            Category = "Medium Risk",
            Tag = "Important",
            Priority = "2",
            MitreAttacks = [
                "T1055.001 - Process Injection: Dynamic-link Library Injection",
                "T1068 - Exploitation for Privilege Escalation",
                "T1078.002 - Valid Accounts: Domain Accounts"
            ],
            Tenants = [],
            CreatedBy = "Mario",
            UpdatedBy = null,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };

        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);
        mockUseCaseRepository.Setup(x => x.UpdateUseCase(It.IsAny<UseCase>(), It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);

        UpdateUseCaseCommand request = new(Guid.NewGuid(), "New UseCase", "New type", "New Status", "New category", "New Tag", "New priority", [], [], "Mario", [], []);

        var resultUseCaseUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.False(resultUseCaseUpdated.IsError);
        Assert.Equal(useCase.Id, resultUseCaseUpdated.Value.Id);
        Assert.Equal(useCase, resultUseCaseUpdated.Value);
    }
}
