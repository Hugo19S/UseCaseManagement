namespace UseCaseManagement.Service.Contract;

public class CreateUseCaseFileRequest
{
    public IFormFile? File { get; set; } = null;
}

public class UseCaseFileResponse
{
    public Guid Id { get; set; }
    public Guid UseCaseId { get; set; }
    public required string FileName { get; set; }
    public int FileSize { get; set; }
    public required string Uri { get; set; }
    public required string Type { get; set; }
}

