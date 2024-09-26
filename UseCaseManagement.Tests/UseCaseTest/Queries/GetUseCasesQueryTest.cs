using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.UseCases.Queries.GetUseCases;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Domain.Filters;

namespace UseCaseManagement.Tests.UseCaseTest.Queries;

public class GetUseCasesQueryTest
{
    [Fact]
    public async void GetUseCasesQuery_Return_UseCaseList()
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
                Tenants = [],
                CreatedBy = "Mario",
                UpdatedBy = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            }
        ];
        var mockRepository = new Mock<IUseCaseRepository>();
        mockRepository.Setup(x => x.GetAllUseCase(It.IsAny<UseCaseFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCases);

        GetUseCasesQueryHandler handler = new(mockRepository.Object);

        UseCaseFilter filter = new();
        GetUseCasesQuery request = new(filter);

        var useCaseResult = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(useCaseResult.Value);
        Assert.IsType<List<UseCase>>(useCaseResult.Value);
        Assert.Equal(useCases, useCaseResult.Value);
    }

    [Fact]
    public async void GetUseCasesQuery_Return_Empty()
    {
        var mockRepository = new Mock<IUseCaseRepository>();
        mockRepository.Setup(x => x.GetAllUseCase(It.IsAny<UseCaseFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

        GetUseCasesQueryHandler handler = new(mockRepository.Object);

        UseCaseFilter filter = new();
        GetUseCasesQuery request = new(filter);

        var useCaseResult = await handler.Handle(request, CancellationToken.None);

        Assert.Empty(useCaseResult.Value);
        Assert.IsType<List<UseCase>>(useCaseResult.Value);
    }
}
