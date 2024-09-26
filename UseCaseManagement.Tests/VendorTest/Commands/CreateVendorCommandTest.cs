using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Vendors.Commands.CreateVendors;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.VendorTest.Commands;

public class CreateVendorCommandTest
{
    [Fact]
    public async void CreateVendorCommand_Return_Error_Vendor_Conflict()
    {
        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.AddVendor(It.IsAny<Vendor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Error.Conflict("Vendor.Conflict", "There is already a vendor with the same name."));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        CreateVendorCommand request = new("New Vendor");

        var vendorResult = await handler.Handle(request, CancellationToken.None);

        Assert.True(vendorResult.IsError);
        Assert.Equal(Error.Conflict("Vendor.Conflict", "There is already a vendor with the same name."), vendorResult.FirstError);
    }

    [Fact]
    public async void CreateVendorCommand_Return_Created()
    {
        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.AddVendor(It.IsAny<Vendor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Created());

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        CreateVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        CreateVendorCommand request = new("New Vendor");

        var vendorResult = await handler.Handle(request, CancellationToken.None);

        Assert.False(vendorResult.IsError);
        Assert.Equal(request.VendorName, vendorResult.Value.Name);
    }
}
