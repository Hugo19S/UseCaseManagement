using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Tenants.Commands.DeleteTenants;
using UseCaseManagement.Application.UseCases.Commands.DeleteUseCases;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.TenantsTest.Commands;

public class DeleteTenantCommandTest
{
    [Fact]
    public async void DeleteUseCaseCommand_Return_NotFound()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetTenant(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        mockUseCaseRepository.Setup(x => x.DeleteTenant(It.IsAny<List<UseCase>>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));


        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteTenantCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        DeleteTenantCommand request = new(Guid.NewGuid());

        var resultTenantDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultTenantDeleted.IsError);
        Assert.Equal(Error.NotFound("Tenant.NotFound", $"Tenant with id {request.TenantId} not found."), resultTenantDeleted.FirstError);
    }

    [Fact]
    public async void DeleteUseCaseCommand_Return_DeletedId()
    {
        Guid tenantId = Guid.NewGuid();

        List<UseCase> useCases =
        [
            new()
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
            }
        ];

        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetTenant(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCases);
        mockUseCaseRepository.Setup(x => x.DeleteTenant(It.IsAny<List<UseCase>>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteTenantCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        DeleteTenantCommand request = new(tenantId);

        var resultTenantDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.False(resultTenantDeleted.IsError);
        Assert.IsType<Guid>(resultTenantDeleted.Value);
        Assert.Equal(tenantId, resultTenantDeleted.Value);
    }
}
