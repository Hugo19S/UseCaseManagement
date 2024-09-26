using ErrorOr;
using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.LogSourceFiles.Queries.GetLogSourceFile;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.LogSourceFileTest.Queries;

public class GetLogSourceFileQueryTest
{
    [Fact]
    public async void GetLogSourceFileQuery_Return_Null()
    {
        var mockRepository = new Mock<ILogSourceFileRepository>();
        mockRepository.Setup(x => x.GetLogSourceFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((LogSourceFile?)null);

        GetLogSourceFileQueryHandler handler = new(mockRepository.Object);
        GetLogSourceFileQuery request = new(Guid.NewGuid());

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(logSourceFileResult.IsError);
        Assert.Equal(Error.NotFound("File.NotFound", $"File with id {request.FileId} not found."), logSourceFileResult.FirstError);
    }

    [Fact]
    public async void GetLogSourceFileQuery_Return_SingleObject()
    {
        FileStream fileStream = File.OpenRead(@"C:\Users\Hugo Furtado\Desktop\Comprovativo de matricula.pdf");
        LogSourceFile file = new()
        {
            Id = Guid.Parse("68d5290a-5b1c-4510-a65a-886e0e5fd927"),
            FileName = "Comprovativo de matricula.pdf",
            FileSize = 75686,
            Uri = @"C:\Users\Hugo Furtado\Desktop\Estagio\ToRedShift\RED.Box.UseCase Files\LogSourcesFiles\22182d29-0613-4eb6-8a75-4ae8e625b872\68d5290a-5b1c-4510-a65a-886e0e5fd927.pdf",
            Type = "application/pdf"
        };

        var mockRepository = new Mock<ILogSourceFileRepository>();
        mockRepository.Setup(x => x.GetLogSourceFileById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(file);
        mockRepository.Setup(x => x.ReadFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(fileStream);

        GetLogSourceFileQueryHandler handler = new(mockRepository.Object);
        GetLogSourceFileQuery request = new(Guid.NewGuid());

        var logSourceFileResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(logSourceFileResult.IsError);
        Assert.IsType<Tuple<FileStream, string, string>>(logSourceFileResult.Value);
        Assert.IsType<FileStream>(logSourceFileResult.Value.Item1);
        Assert.IsType<string>(logSourceFileResult.Value.Item2);
        Assert.IsType<string>(logSourceFileResult.Value.Item3);
    }
}
