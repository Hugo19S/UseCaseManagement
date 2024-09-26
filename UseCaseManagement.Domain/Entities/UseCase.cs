namespace UseCaseManagement.Domain.Entities;

public class UseCase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public string? Tag { get; set; }
    public string Priority { get; set; }
    public List<string> MitreAttacks { get; set; } = [];
    public List<Guid> Tenants { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<UseCaseFile> Files { get; set; } = [];
    public List<LogSource> LogSources { get; set; } = [];
    public List<Vendor> Vendors { get; set; } = [];
}
