using ErrorOr;
using Moq;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Application.Vendors.Commands.DeleteVendors;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Tests.VendorTest.Commands;

public class DeleteVendorCommandTest
{
    [Fact]
    public async void DeleteVendorCommand_Return_NotFound()
    {
        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.DeleteVendor(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        DeleteVendorCommand request = new(Guid.NewGuid());

        var resultVendorDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.True(resultVendorDeleted.IsError);
        Assert.Equal(Error.NotFound("Vendor.NotFound", $"Vendor with id {request.Id} not found."), resultVendorDeleted.FirstError);
    }
    
    [Fact]
    public async void DeleteVendorCommand_Return_DeletedId()
    {
        Vendor vendorTest = new() 
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        };

        var mockVendorRepository = new Mock<IVendorRepository>();
        mockVendorRepository.Setup(x => x.DeleteVendor(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        mockVendorRepository.Setup(x => x.GetVendorById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(vendorTest);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        DeleteVendorCommandHandler handler = new(mockVendorRepository.Object, mockUnitOfWork.Object);

        DeleteVendorCommand request = new(vendorTest.Id);

        var resultVendorDeleted = await handler.Handle(request, CancellationToken.None);

        Assert.IsType<Guid>(resultVendorDeleted.Value);
        Assert.Equal(vendorTest.Id, resultVendorDeleted.Value);
    }
}
