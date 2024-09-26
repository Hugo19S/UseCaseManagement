namespace UseCaseManagement.Domain.Entities;

public class LogSourceFile
{
    public Guid Id { get; set; }
    public Guid LogSourceId { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public string Uri { get; set; }
    public string Type { get; set; }
    public LogSource LogSource { get; set; }
}
