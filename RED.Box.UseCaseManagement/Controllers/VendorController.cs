using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCaseManagement.Application.Vendors.Commands.CreateVendors;
using UseCaseManagement.Application.Vendors.Commands.DeleteVendors;
using UseCaseManagement.Application.Vendors.Commands.UpdateVendors;
using UseCaseManagement.Application.Vendors.Queries.GetVendor;
using UseCaseManagement.Application.Vendors.Queries.GetVendors;
using UseCaseManagement.Service.Contract;

namespace UseCaseManagement.Service.Controllers;

[Route("api/[controller]")]
public class VendorController(ISender sender, IMapper mapper) : ApiController
{
    [HttpGet]
    public async Task<ActionResult> GetVendors(CancellationToken cancellationToken)
    {
        var vendorsOr = await sender.Send(new GetVendorsQuery(), cancellationToken);

        return vendorsOr.Match(
            v => Ok(mapper.Map<IEnumerable<VendorResponse>>(v)), 
            Problem);
    }

    [HttpGet("{vendorId:guid}")]
    public async Task<ActionResult> GetVendor(Guid vendorId, CancellationToken cancellationToken)
    {
        var vendorOr = await sender.Send(new GetVendorQuery(vendorId), cancellationToken);

        return vendorOr.Match(
           v => Ok(mapper.Map<VendorResponseWithDetails>(v)), 
            Problem);
    }

    [HttpPost]
    public async Task<ActionResult> CreateVendor([FromBody] CreateVendorRequest vendorRequest, CancellationToken cancellationToken)
    {
        var vendorCreated = await sender.Send(new CreateVendorCommand(vendorRequest.Name), cancellationToken);
        
        return vendorCreated.Match(
            v => Created("", mapper.Map<VendorResponse>(v)), 
            Problem);
    }

    [HttpDelete("{vendorId:guid}")]
    public async Task<ActionResult> DeleteVendor(Guid vendorId, CancellationToken cancellationToken)
    {
        var vendorDeleted = await sender.Send(new DeleteVendorCommand(vendorId), cancellationToken);

        return vendorDeleted.Match(
            v => Ok($"Vendor with Id {v} has been deleted."), 
            Problem);
    }

    [HttpPut("{vendorId:guid}")]
    public async Task<ActionResult> UpdateVendor(Guid vendorId, [FromBody] UpdateVendorRequest vendorRequest)
    {
        var vendorOr = await sender.Send(new UpdateVendorCommand(vendorId, vendorRequest.Name));

        return vendorOr.Match(
            v => Ok(mapper.Map<VendorResponse>(v)), 
            Problem);
    }
}
