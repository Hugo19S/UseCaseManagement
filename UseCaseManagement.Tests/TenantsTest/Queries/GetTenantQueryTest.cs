using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Tenants.Queries.GetTenant;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.TenantsTest.Queries;

public class GetTenantQueryTest
{
    [Fact]
    public async void GetTenantQuery_Return_Empty()
    {
        var repoMock = new Mock<IUseCaseRepository>();
        repoMock.Setup(x => x.GetTenant(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

        GetTenantQueryHandler handler = new(repoMock.Object);
        GetTenantQuery request = new(Guid.NewGuid());

        var useCaseWithTenantResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseWithTenantResult.IsError);
        Assert.Empty(useCaseWithTenantResult.Value);
        Assert.IsType<List<UseCase>>(useCaseWithTenantResult.Value);
    }

    [Fact]
    public async void GetTenantQuery_Return_UseCaseList()
    {
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
                Tenants = [
                    Guid.Parse("e021475f-b454-47b7-8f39-f22bea5f2788"),
                    Guid.Parse("d021475f-b454-57b7-8f39-f22bea5f2752")
                ],
                CreatedBy = "Mario",
                UpdatedBy = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            }
        ];

        var repoMock = new Mock<IUseCaseRepository>();
        repoMock.Setup(x => x.GetTenant(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCases);

        GetTenantQueryHandler handler = new(repoMock.Object);
        GetTenantQuery request = new(Guid.NewGuid());

        var useCaseWithTenantResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseWithTenantResult.IsError);
        Assert.NotEmpty(useCaseWithTenantResult.Value);
        Assert.IsType<List<UseCase>>(useCaseWithTenantResult.Value);
        Assert.Equal(useCases, useCaseWithTenantResult.Value);
    }

}
