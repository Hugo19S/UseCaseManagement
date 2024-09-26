using UseCaseManagement.Domain;

namespace UseCaseManagement.Service.Contract;

public class UpdateLogSourceRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Guid> UseCases { get; set; } = [];
}

public class UpdateLogSourceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<UseCaseResponse> UseCases { get; set; } = [];
}
