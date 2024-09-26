using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Tenants.Commands.AddTenantToUseCase;
using UseCaseManagement.Application.Tenants.Commands.DeleteTenants;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.TenantsTest.Commands;

public class AddTenantToUseCaseCommandTest
{
    [Fact]
    public async void AddTenantToUseCaseCommandTest_Return_NotFound()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);
        mockUseCaseRepository.Setup(x => x.AddTenantToUseCase(It.IsAny<UseCase>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        AddTenantToUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        AddTenantToUseCaseCommand request = new(Guid.NewGuid(), Guid.NewGuid());

        var resultTenantAdded = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultTenantAdded.IsError);
        Assert.Equal(Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found."), resultTenantAdded.FirstError);
    }

    [Fact]
    public async void AddTenantToUseCaseCommandTest_Return_Conflit()
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
            Tenants = [
                Guid.Parse("d9afa58c-53c9-4886-9898-5d5ce742c8c1"),
                Guid.Parse("29165f4d-73bd-4ef5-a096-ba7f48808c93"),
                Guid.Parse("4cf22a7f-b421-4ee8-b0f0-82fafe044410"),
            ],
            CreatedBy = "Mario",
            UpdatedBy = null,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };

        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);
        mockUseCaseRepository.Setup(x => x.AddTenantToUseCase(It.IsAny<UseCase>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        AddTenantToUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        AddTenantToUseCaseCommand request = new(Guid.Parse("e021475f-b454-47b7-8f39-f22bea5f2788"), Guid.Parse("d9afa58c-53c9-4886-9898-5d5ce742c8c1"));

        var resultTenantAdded = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultTenantAdded.IsError);
        Assert.Equal(Error.Conflict("Tenants.Conflit", $"UseCase with id {request.UseCaseId} contain a tenant with id {request.TenantId}."), resultTenantAdded.FirstError);
    }
    
    [Fact]
    public async void AddTenantToUseCaseCommandTest_Return_TenantAddedToUseCase()
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
            Tenants = [
                Guid.Parse("d9afa58c-53c9-4886-9898-5d5ce742c8c1"),
                Guid.Parse("29165f4d-73bd-4ef5-a096-ba7f48808c93"),
                Guid.Parse("4cf22a7f-b421-4ee8-b0f0-82fafe044410"),
            ],
            CreatedBy = "Mario",
            UpdatedBy = null,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };

        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);
        mockUseCaseRepository.Setup(x => x.AddTenantToUseCase(It.IsAny<UseCase>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        AddTenantToUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        AddTenantToUseCaseCommand request = new(Guid.Parse("e021475f-b454-47b7-8f39-f22bea5f2788"), Guid.NewGuid());

        var resultTenantAdded = await handler.Handle(request, CancellationToken.None);

        Assert.False(resultTenantAdded.IsError);
        Assert.IsType<UseCase>(resultTenantAdded.Value);
        Assert.Equal(useCase, resultTenantAdded.Value);
    }
}
