namespace UseCaseManagement.Domain.Filters;

public class UseCaseFilter
{
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
    public string? Tag { get; set; }
    public string? Priority { get; set; }
}
