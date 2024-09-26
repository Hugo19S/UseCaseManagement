namespace UseCaseManagement.Domain.Entities;

public class UseCaseFile
{
    public Guid Id { get; set; }
    public Guid UseCaseId { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public string Uri { get; set; }
    public string Type { get; set; }
    public UseCase UseCase { get; set; }
}
