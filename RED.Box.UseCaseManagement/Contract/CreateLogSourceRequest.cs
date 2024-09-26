using UseCaseManagement.Domain;

namespace UseCaseManagement.Service.Contract;

public class CreateLogSourceRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class LogSourceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class LogSourceResponseWithDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<LogSourceFileResponseToDetails> Files { get; set; } = [];
    public List<UseCaseResponse> UseCases { get; set; } = [];
}

public class LogSourceFileResponseToDetails
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public string Uri { get; set; }
    public string Type { get; set; }
}
