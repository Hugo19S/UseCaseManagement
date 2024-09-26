using UseCaseManagement.Domain;

namespace UseCaseManagement.Service.Contract;

public class CreateUseCaseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Tag { get; set; }
    public string Priority { get; set; } = string.Empty;
    public List<string> MitreAttacks { get; set; } = [];
    public List<Guid> Tenants { get; set; } = [];
}

public class UseCaseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public string? Tag { get; set; }
    public string Priority { get; set; }
    public List<string> MitreAttacks { get; set; }
    public List<Guid> Tenants { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
public class UseCaseResponseWithDetails
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public string? Tag { get; set; }
    public string Priority { get; set; }
    public List<string> MitreAttacks { get; set; } = [];
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<Guid> Tenants { get; set; }
    public List<UseCaseFileResponseToDetails> Files { get; set; } = [];
    public List<LogSourceResponse> LogSources { get; set; } = [];
    public List<VendorResponse> Vendors { get; set; } = [];
}

public class UseCaseFileResponseToDetails
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public string Uri { get; set; }
    public string Type { get; set; }
}