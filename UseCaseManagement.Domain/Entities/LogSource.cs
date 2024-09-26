namespace UseCaseManagement.Domain.Entities;

public class LogSource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<LogSourceFile> Files { get; set; } = [];
    public List<UseCase> UseCases { get; set; } = [];
}