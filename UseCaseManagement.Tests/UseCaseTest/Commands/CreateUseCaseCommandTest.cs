using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSources.Commands.CreateLogSources;
using UseCaseManagement.Application.UseCases.Commands.CreateUseCases;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseTest.Commands;

public class CreateUseCaseCommandTest
{
    [Fact]
    public async void CreateUseCaseCommand_Return_Error_UseCase_Conflict()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.AddUseCase(It.IsAny<UseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        CreateUseCaseCommand request = new("New UseCase", "New type", "New Status", "New category", "New Tag", "New priority", [], [], "Mario");

        var useCaseResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(useCaseResult.IsError);
        Assert.Equal(Error.Conflict("UseCase.Conflict", "There is already a UseCase with the same Title."), useCaseResult.FirstError);
    }

    [Fact]
    public async void CreateUseCaseCommand_Return_Created()
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
        mockUseCaseRepository.Setup(x => x.AddUseCase(It.IsAny<UseCase>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        CreateUseCaseCommand request = new(useCase.Title, useCase.Type, useCase.Status, useCase.Category, useCase.Tag, useCase.Priority, useCase.MitreAttacks, useCase.Tenants, useCase.CreatedBy);

        var useCaseResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseResult.IsError);
        Assert.Equal(request.Title, useCaseResult.Value.Title);
    }
}
