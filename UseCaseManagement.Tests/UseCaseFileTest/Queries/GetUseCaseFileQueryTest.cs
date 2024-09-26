using ErrorOr;
using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.UseCaseFiles.Queries.GetUseCaseFile;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.UseCaseFileTest.Queries;

public class GetUseCaseFileQueryTest
{
    [Fact]
    public async void GetUseCaseFileQuery_Return_Null()
    {
        var mockRepository = new Mock<IUseCaseFileRepository>();
        mockRepository.Setup(x => x.GetUseCaseFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((UseCaseFile?)null);

        GetUseCaseFileQueryHandler handler = new(mockRepository.Object);
        GetUseCaseFileQuery request = new(Guid.NewGuid());

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(useCaseFileResult.IsError);
        Assert.Equal(Error.NotFound("File.NotFound", $"File with id {request.FileId} not found."), useCaseFileResult.FirstError);
    }

    [Fact]
    public async void GetUseCaseFileQuery_Return_SingleObject()
    {
        FileStream fileStream = File.OpenRead(@"C:\Users\Hugo Furtado\Desktop\Comprovativo de matricula.pdf");
        UseCaseFile file = new()
        {
            Id = Guid.Parse("0783d097-61af-4744-8277-06d6473ea413"),
            FileName = "Comprovativo de matricula.pdf",
            FileSize = 75686,
            Uri = @"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\UseCaseFiles\2cf772a4-4a42-4b0d-9dea-cd7631563166\0783d097-61af-4744-8277-06d6473ea413.pdf",
            Type = "application/pdf"
        };

        var mockRepository = new Mock<IUseCaseFileRepository>();
        mockRepository.Setup(x => x.GetUseCaseFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(file);
        mockRepository.Setup(x => x.ReadFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(fileStream);

        GetUseCaseFileQueryHandler handler = new(mockRepository.Object);
        GetUseCaseFileQuery request = new(Guid.NewGuid());

        var useCaseFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(useCaseFileResult.IsError);
        Assert.IsType<Tuple<FileStream, string, string>>(useCaseFileResult.Value);
        Assert.IsType<FileStream>(useCaseFileResult.Value.Item1);
        Assert.IsType<string>(useCaseFileResult.Value.Item2);
        Assert.IsType<string>(useCaseFileResult.Value.Item3);
    }
}
