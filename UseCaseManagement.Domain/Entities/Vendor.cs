namespace UseCaseManagement.Domain.Entities;

public class Vendor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UseCase> UseCases { get; set; }
}
