using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Vendors.Queries.GetVendors;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.VendorTest.Queries;

public class GetVendorsQueryTest
{
    [Fact]
    public async void GetVendorsQuery_Return_VendorList()
    {
        List<Vendor> vendors =
        [
            new(){
                Id = Guid.Parse("24e0a196-8d88-4e45-9a1d-6c1e6fe5ecc8"),
                Name = "Microsoft",
            },
            new()
            {
                Id = Guid.Parse("c8a58f77-06bb-47bd-b0de-1960fa39ea0e"),
                Name = "Cisco"
            }
        ];

        var mockRepository = new Mock<IVendorRepository>();
        mockRepository.Setup(x => x.GetAllVendor(It.IsAny<CancellationToken>())).ReturnsAsync(vendors);

        GetVendorsQueryHandler handler = new(mockRepository.Object);

        GetVendorsQuery query = new();

        var vendorsResult = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(vendorsResult.Value);
        Assert.IsType<List<Vendor>>(vendorsResult.Value);
        Assert.Equal(vendors, vendorsResult.Value);
    }

    [Fact]
    public async void GetVendorsQuery_Return_Empty()
    {
        var mockRepository = new Mock<IVendorRepository>();
        mockRepository.Setup(x => x.GetAllVendor(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        GetVendorsQueryHandler handler = new(mockRepository.Object);

        GetVendorsQuery query = new();

        var vendorsResult = await handler.Handle(query, CancellationToken.None);

        Assert.Empty(vendorsResult.Value);
        Assert.IsType<List<Vendor>>(vendorsResult.Value);
    }
}
