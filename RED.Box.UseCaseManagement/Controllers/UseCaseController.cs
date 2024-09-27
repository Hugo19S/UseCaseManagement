using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using UseCaseManagement.Application.UseCaseFiles.Commands.CreateUseCaseFiles;
using UseCaseManagement.Application.UseCaseFiles.Commands.DeleteUseCaseFiles;
using UseCaseManagement.Application.UseCaseFiles.Queries.GetUseCaseFile;
using UseCaseManagement.Application.UseCases.Commands.CreateUseCases;
using UseCaseManagement.Application.UseCases.Commands.DeleteUseCases;
using UseCaseManagement.Application.UseCases.Commands.UpdateUseCases;
using UseCaseManagement.Application.UseCases.Queries.GetUseCase;
using UseCaseManagement.Application.UseCases.Queries.GetUseCases;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Filters;
using UseCaseManagement.Service.Contract;

namespace UseCaseManagement.Service.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "usecase_viewer")]
public class UseCaseController(ISender sender, IMapper mapper) : ApiController
{
    [HttpGet]
    public async Task<ActionResult> GetUseCases([FromQuery] UseCaseFilter useCaseFilter, CancellationToken cancellationToken)
    {
        var vendorsOr = await sender.Send(new GetUseCasesQuery(useCaseFilter), cancellationToken);
        return vendorsOr.Match(
            v => Ok(mapper.Map<IEnumerable<UseCaseResponse>>(v)), 
            Problem);
    }

    [HttpGet("{useCaseId:guid}")]
    public async Task<ActionResult> GetUseCase(Guid useCaseId, CancellationToken cancellationToken)
    {
        var vendorOr = await sender.Send(new GetUseCaseQuery(useCaseId), cancellationToken);
        return vendorOr.Match(
            v => Ok(mapper.Map<UseCaseResponseWithDetails>(v)),
            Problem);
    }

    [HttpPost]
    [Authorize(Roles = "usecase_manager")]
    public async Task<ActionResult> CreateUseCase([FromBody] CreateUseCaseRequest useCaseRequest, CancellationToken cancellationToken)
    {
        var createdBy = HttpContext.User.Claims.ToList().ElementAt(5).Value;

        var useCaseCreated = await sender.Send(
            new CreateUseCaseCommand(useCaseRequest.Title, useCaseRequest.Type, 
                                     useCaseRequest.Status, useCaseRequest.Category, 
                                     useCaseRequest.Tag, useCaseRequest.Priority, 
                                     useCaseRequest.MitreAttacks, useCaseRequest.Tenants, createdBy), cancellationToken);

        return useCaseCreated.Match(
            v => Created("", mapper.Map<UseCaseResponse>(v)), 
            Problem);
    }

    [HttpDelete("{useCaseId:guid}")]
    [Authorize(Roles = "usecase_manager")]
    public async Task<ActionResult> DeleteUseCase(Guid useCaseId, CancellationToken cancellationToken)
    {
        var vendorDeleted = await sender.Send(new DeleteUseCaseCommand(useCaseId), cancellationToken);

        return vendorDeleted.Match(
            v => Ok($"UseCase with Id {v} has been deleted."),
            Problem);
    }

    [HttpPut("{useCaseId:guid}")]
    [Authorize(Roles = "usecase_manager")]
    public async Task<ActionResult> UpdateUseCase(Guid useCaseId, [FromBody] UpdateUseCaseRequest updateUseCase)
    {
        var updatedBy = HttpContext.User.Claims.ToList().ElementAt(5).Value;

        var vendorOr = await sender.Send(new UpdateUseCaseCommand(useCaseId, updateUseCase.Title, updateUseCase.Type, updateUseCase.Status, 
                                                                  updateUseCase.Category, updateUseCase.Tag, updateUseCase.Priority,
                                                                  updateUseCase.MitreAttacks, updateUseCase.Tenants, updatedBy,
                                                                  updateUseCase.LogSources, updateUseCase.Vendors));

        return vendorOr.Match(Ok, Problem);
    }

    [HttpPost("post/{useCaseId:guid}")]
    [Authorize(Roles = "usecase_manager")]
    public async Task<ActionResult> CreateFile(Guid useCaseId, [FromForm] CreateUseCaseFileRequest useCaseRequest)
    {
        FileRepresentation? fileRepresentation = null;

        if (useCaseRequest.File != null)
        {
            fileRepresentation = new FileRepresentation
            {
                Name = useCaseRequest.File!.FileName,
                Content = useCaseRequest.File.OpenReadStream(),
                Size = useCaseRequest.File.Length,
                ContentType = useCaseRequest.File.ContentType
            };
        }

        var fileCreatedOr = await sender.Send(new CreateUseCaseFileCommand(useCaseId, fileRepresentation));
        return fileCreatedOr.Match(
            v => Created("", mapper.Map<UseCaseFileResponse>(v)),
            Problem);
    }

    [HttpGet("download/{fileId}")]
    public async Task<ActionResult> DownloadFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileStream = await sender.Send(new GetUseCaseFileQuery(fileId), cancellationToken);

        return fileStream.Match(v =>
        {
            ContentDisposition contentDisposition = new()
            {
                FileName = v.Item2,
                Size = v.Item1.Length
            };

            Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

            return File(v.Item1, v.Item3);
        },
        Problem);
    }

    [HttpDelete("file/{fileId}")]
    [Authorize(Roles = "usecase_manager")]
    public async Task<ActionResult> DeleteFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileDeleted = await sender.Send(new DeleteUseCaseFileCommand(fileId), cancellationToken);

        return fileDeleted.Match(v => Ok($"File with Id {v} has been deleted."), Problem);
    }
}
