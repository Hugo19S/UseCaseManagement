using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.UseCases.Commands.DeleteUseCases;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseTest.Commands;

public class DeleteUseCaseCommandTest
{
    [Fact]
    public async void DeleteUseCaseCommand_Return_NotFound()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.DeleteUseCase(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        DeleteUseCaseCommand request = new(Guid.NewGuid());

        var resultUseCaseDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultUseCaseDeleted.IsError);
        Assert.Equal(Error.NotFound("UseCase.NotFound", $"UseCase with id {request.Id} not found."), resultUseCaseDeleted.FirstError);
    }

    [Fact]
    public async void DeleteUseCaseCommand_Return_DeletedId()
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
        mockUseCaseRepository.Setup(x => x.DeleteUseCase(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(useCase);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteUseCaseCommandHandler handler = new(mockUseCaseRepository.Object, mockUnitOfWork.Object);
        DeleteUseCaseCommand request = new(useCase.Id);

        var resultVendorDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.IsType<Guid>(resultVendorDeleted.Value);
        Assert.Equal(useCase.Id, resultVendorDeleted.Value);
    }
}
