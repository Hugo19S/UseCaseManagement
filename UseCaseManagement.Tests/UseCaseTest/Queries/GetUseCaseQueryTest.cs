using ErrorOr;
using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.UseCases.Queries.GetUseCase;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseTest.Queries;

public class GetUseCaseQueryTest
{
    [Fact]
    public async void GetUseCaseQuery_Return_Null()
    {
        var mockRepository = new Mock<IUseCaseRepository>();
        mockRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);

        GetUseCaseQueryHandler handler = new(mockRepository.Object);
        GetUseCaseQuery request = new(Guid.NewGuid());

        var useCasesResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(useCasesResult.IsError);
        Assert.Equal(Error.NotFound("UseCase.NotFound", $"UseCase with id {request.Id} not found."), useCasesResult.FirstError);
    }

    [Fact]
    public async void GetUseCaseQuery_Return_SingleObject()
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

        var repoMock = new Mock<IUseCaseRepository>();
        repoMock.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);

        GetUseCaseQueryHandler handler = new(repoMock.Object);

        GetUseCaseQuery request = new(Guid.NewGuid());
        var useCaseResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseResult.IsError);
        Assert.Equal(useCase, useCaseResult.Value);
    }
}
