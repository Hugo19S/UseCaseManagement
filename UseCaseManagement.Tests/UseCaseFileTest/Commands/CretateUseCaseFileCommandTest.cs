using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSourceFiles.Commands.CreateLogSourceFiles;
using UseCaseManagement.Application.UseCaseFiles.Commands.CreateUseCaseFiles;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseFileTest.Commands;

public class CretateUseCaseFileCommandTest
{
    [Fact]
    public async void CreateUseCaseFileCommand_Return_UseCase_NotFound()
    {
        var mockUseCaseRepository = new Mock<IUseCaseRepository>();
        mockUseCaseRepository.Setup(x => x.GetUseCaseById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCase?)null);

        var mockUseCaseFileRepository = new Mock<IUseCaseFileRepository>();
        mockUseCaseFileRepository.Setup(x => x.SaveFile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<FileRepresentation>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        mockUseCaseFileRepository.Setup(x => x.AddUseCaseFile(It.IsAny<UseCaseFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateUseCaseFileCommandHandler handler = new(mockUseCaseFileRepository.Object, mockUseCaseRepository.Object, mockUnitOfWork.Object);

        FileRepresentation? fileRepresentation = null;
        CreateUseCaseFileCommand request = new(Guid.NewGuid(), fileRepresentation);

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(useCaseFileResult.IsError);
        Assert.Equal(Error.NotFound("UseCase.NotFound", $"UseCase with id {request.UseCaseId} not found."), useCaseFileResult.FirstError);
    }

    [Fact]
    public async void CreateUseCaseFileCommand_Return_Created()
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

        var mockUseCaseFileRepository = new Mock<IUseCaseFileRepository>();
        mockUseCaseFileRepository.Setup(x => x.SaveFile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<FileRepresentation>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        mockUseCaseFileRepository.Setup(x => x.AddUseCaseFile(It.IsAny<UseCaseFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateUseCaseFileCommandHandler handler = new(mockUseCaseFileRepository.Object, mockUseCaseRepository.Object, mockUnitOfWork.Object);

        FileRepresentation fileRepresentation = new()
        {
            Name = "Comprovativo de matricula.pdf",
            ContentType = "application/pdf",
            Size = 75686,
            Content = new FileStream(@"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\UseCaseFiles\2cf772a4-4a42-4b0d-9dea-cd7631563166\0783d097-61af-4744-8277-06d6473ea413.pdf", FileMode.Open)
        };
        CreateUseCaseFileCommand request = new(Guid.NewGuid(), fileRepresentation);

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseFileResult.IsError);
        Assert.Equal(request.File!.Name, useCaseFileResult.Value.FileName);
    }

}
