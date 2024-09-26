using UseCaseManagement.Domain;

namespace UseCaseManagement.Service.Contract;

public class CreateLogSourceFileRequest
{
    public IFormFile? File { get; set; } = null;
}

public class LogSourceFileResponse
{
    public Guid Id { get; set; }
    public Guid LogSourceId { get; set; }
    public string? FileName { get; set; }
    public int FileSize { get; set; }
    public string? Uri { get; set; }
    public string? Type { get; set; }
}
