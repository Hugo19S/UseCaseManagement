namespace UseCaseManagement.Service.Contract;

public class UpdateUseCaseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? Tag { get; set; }
    public List<string> MitreAttacks { get; set; } = [];
    public List<Guid> Tenants { get; set; } = [];
    public List<Guid> LogSources { get; set; } = [];
    public List<Guid> Vendors { get; set; } = [];
}

public class UpdateUseCaseResponse
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Tag { get; set; }
    public string Priority { get; set; } = string.Empty;
    public List<string> MitreAttacks { get; set; } = [];
    public List<Guid> LogSources { get; set; } = [];
    public List<Guid> Vendors { get; set; } = [];
}
