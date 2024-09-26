using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Vendors.Commands.UpdateVendors;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.VendorTest.Commands;

public class UpdateVendorCommandTest
{
    [Fact]
    public async void UpdateVendorQuery_Return_NotFound()
    {
        Vendor vendorTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };

        var mockVendorRepository = new Mock<IVendorRepository>();

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockVendorRepository.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Vendor?)null);
        mockVendorRepository.Setup(x => x.UpdateVendor(It.IsAny<Vendor>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendorTest);

        UpdateVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        UpdateVendorCommand request = new(vendorTest.Id, "New test");

        var resultVendorUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultVendorUpdated.IsError);
        Assert.Equal(Error.NotFound("Vendor.NotFound", $"Vendor with id {request.VendorId} not found."), resultVendorUpdated.FirstError);
    }
    
    [Fact]
    public async void UpdateVendorQuery_Return_Conflit()
    {
        Vendor vendorTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };

        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendorTest);
        mockVendorRepository.Setup(x => x.UpdateVendor(It.IsAny<Vendor>(), It.IsAny<CancellationToken>())).ReturnsAsync((Vendor?)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        UpdateVendorCommand request = new(vendorTest.Id, "New test");

        var resultVendorUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultVendorUpdated.IsError);
        Assert.Equal(Error.Conflict("Product.Conflict", "There is already a vendor with the same name."), resultVendorUpdated.FirstError);
    }
    
    [Fact]
    public async void UpdateVendorQuery_Return_Vendor()
    {
        Vendor vendorTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };

        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendorTest);
        mockVendorRepository.Setup(x => x.UpdateVendor(It.IsAny<Vendor>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendorTest);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        UpdateVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        UpdateVendorCommand request = new(vendorTest.Id, "New test");

        var resultVendorUpdated = await handler.Handle(request, CancellationToken.None);

        Assert.False(resultVendorUpdated.IsError);
        Assert.Equal(vendorTest.Id, resultVendorUpdated.Value.Id);
    }
}
