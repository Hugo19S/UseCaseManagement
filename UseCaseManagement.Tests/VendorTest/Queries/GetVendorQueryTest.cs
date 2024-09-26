using ErrorOr;
using Moq;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Vendors.Queries.GetVendor;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.VendorTest.Queries;

public class GetVendorQueryTest
{
    [Fact]
    public async void GetVendorQuery_Return_Null()
    {
        var repoMock = new Mock<IVendorRepository>();
        repoMock.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Vendor?)null);

        GetVendorQueryHandler handler = new(repoMock.Object);

        GetVendorQuery request = new(Guid.NewGuid());
        var vendorResult = await handler.Handle(request, CancellationToken.None);


        Assert.True(vendorResult.IsError);

        Error error = vendorResult.FirstError;
        Assert.Equal(Error.NotFound("Vendor.NotFound", $"Vendor with id {request.VendorId} not found."), error);
    }

    [Fact]
    public async void GetVendorQuery_Return_SingleObject()
    {
        Vendor vendor = new()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
        };

        var repoMock = new Mock<IVendorRepository>();
        repoMock.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendor);

        GetVendorQueryHandler handler = new(repoMock.Object);

        GetVendorQuery request = new(Guid.NewGuid());
        var vendorResult = await handler.Handle(request, CancellationToken.None);


        Assert.False(vendorResult.IsError);

        Assert.Equal(vendor, vendorResult.Value);
    }
}
