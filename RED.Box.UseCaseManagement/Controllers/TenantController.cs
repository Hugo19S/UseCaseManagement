using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCaseManagement.Application.Tenants.Commands.AddTenantToUseCase;
using UseCaseManagement.Application.Tenants.Commands.DeleteTenantFromUseCase;
using UseCaseManagement.Application.Tenants.Commands.DeleteTenants;
using UseCaseManagement.Application.Tenants.Queries.GetTenant;
using UseCaseManagement.Service.Contract;

namespace UseCaseManagement.Service.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "tenant_viewer")]
public class TenantController(ISender sender, IMapper mapper) : ApiController
{
    [HttpGet("{tenantID:guid}")]
    public async Task<ActionResult> GetTenant(Guid tenantID, CancellationToken cancellationToken)
    {
        var useCaseOr = await sender.Send(new GetTenantQuery(tenantID), cancellationToken);

        return useCaseOr.Match(
            v => Ok(mapper.Map<IEnumerable<UseCaseResponse>>(v)), 
            Problem);
    }

    [HttpDelete("{tenantId:guid}")]
    [Authorize(Roles = "tenant_manager")]
    public async Task<ActionResult> DeleteTenant(Guid tenantId, CancellationToken cancellationToken)
    {
        var tenantDeleted = await sender.Send(new DeleteTenantCommand(tenantId), cancellationToken);
        return tenantDeleted.Match(v => Ok(), Problem);
    }

    /* Metodos para UseCaseControlle */

    [HttpPost("{useCaseId:guid}/{tenantId:guid}")]
    [Authorize(Roles = "tenant_manager")]
    public async Task<ActionResult> AddTenantToUseCase(Guid useCaseId, Guid tenantId, CancellationToken cancellationToken)
    {
        var tenantToUseCase = await sender.Send(new AddTenantToUseCaseCommand( useCaseId, tenantId), cancellationToken);

        return tenantToUseCase.Match(
            v => Ok(mapper.Map<UseCaseResponse>(v)), 
            Problem);
    }
    
    [HttpDelete("{useCaseId:guid}/{tenantId:guid}")]
    [Authorize(Roles = "tenant_manager")]
    public async Task<ActionResult> DeleteTenantFromUseCase(Guid useCaseId, Guid tenantId, CancellationToken cancellationToken)
    {
        var tenantFromUseCaseDeleted = await sender.Send(new DeleteTenantUseCaseCommand( useCaseId, tenantId), cancellationToken);

        return tenantFromUseCaseDeleted.Match(v => Ok(), Problem);
    }
}
