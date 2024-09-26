using UseCaseManagement.Domain;

namespace UseCaseManagement.Service.Contract;

public class CreateVendorRequest
{
    public string Name { get; set; } = string.Empty;
}

public class VendorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class VendorResponseWithDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UseCaseResponse> UseCases { get; set; }
}

public class UpdateVendorRequest
{
    public string Name { get; set; } = string.Empty;
}
