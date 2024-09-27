using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using UseCaseManagement.Application.LogSourceFiles.Commands.CreateLogSourceFiles;
using UseCaseManagement.Application.LogSourceFiles.Commands.DeleteLogSourceFiles;
using UseCaseManagement.Application.LogSourceFiles.Queries.GetLogSourceFile;
using UseCaseManagement.Application.LogSources.Commands.CreateLogSources;
using UseCaseManagement.Application.LogSources.Commands.DeleteLogSources;
using UseCaseManagement.Application.LogSources.Commands.UpdateLogSources;
using UseCaseManagement.Application.LogSources.Queries.GetLogSource;
using UseCaseManagement.Application.LogSources.Queries.GetLogSources;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Service.Contract;

namespace UseCaseManagement.Service.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "logsource_viewer")]
public class LogSourceController(ISender sender, IMapper mapper) : ApiController
{
    [HttpGet]
    
    public async Task<ActionResult> GetLogSources()
    {
        var logSource = await sender.Send(new GetLogSourcesQuery());

        return logSource.Match(
            v => Ok(mapper.Map<IEnumerable<LogSourceResponse>>(v)), 
            Problem);
    }

    [HttpGet("{logSourceId:guid}")]
    public async Task<ActionResult> GetLogSource(Guid logSourceId)
    {
        var logSource = await sender.Send(new GetLogSourceQuery(logSourceId));

        return logSource.Match(v => Ok(mapper.Map<LogSourceResponseWithDetails>(v)), Problem);
    }

    [HttpPost]
    [Authorize(Roles = "logsource_manager")]
    public async Task<ActionResult> CreateLogSource([FromBody] CreateLogSourceRequest logSourceRequest)
    {
        var logSourceCreated = await sender.Send(new CreateLogSourceCommand(logSourceRequest.Name, logSourceRequest.Description));

        return logSourceCreated.Match(
            v=> Ok(mapper.Map<LogSourceResponse>(v)), 
            Problem);
    }

    [HttpPut("{logSourceId:guid}")]
    [Authorize(Roles = "logsource_manager")]
    public async Task<ActionResult> UpdateLogSource(Guid logSourceId, [FromBody] UpdateLogSourceRequest sourceRequest)
    {
        var logSourceOr = await sender.Send(new UpdateLogSourceCommand(logSourceId, sourceRequest.Name,
                                                                       sourceRequest.Description, sourceRequest.UseCases));

        return logSourceOr.Match(
            v => Ok(mapper.Map<UpdateLogSourceResponse>(v)), 
            Problem);
    }

    [HttpDelete("{logSourceId:guid}")]
    [Authorize(Roles = "logsource_manager")]
    public async Task<ActionResult> DeleteLogSource(Guid logSourceId)
    {
        var logSourceDeleted = await sender.Send(new DeleteLogSourceCommand(logSourceId));

        return logSourceDeleted.Match(
            v => Ok($"LogSource with Id {v} has been deleted."),
            Problem);
    }

    [HttpPost("post/{logSourceId:guid}")]
    [Authorize(Roles = "logsource_manager")]
    public async Task<ActionResult> CreateFile(Guid logSourceId, [FromForm] CreateLogSourceFileRequest logSourceRequest)
    {
        FileRepresentation? fileRepresentation = null;

        if (logSourceRequest.File != null)
        {
            fileRepresentation = new FileRepresentation
            {
                Name = logSourceRequest.File!.FileName,
                Content = logSourceRequest.File.OpenReadStream(),
                Size = logSourceRequest.File.Length,
                ContentType = logSourceRequest.File.ContentType
            };
        }

        var fileCreatedOr = await sender.Send(new CreateLogSourceFileCommand(logSourceId, fileRepresentation));
        return fileCreatedOr.Match(
            v => Created("", mapper.Map<LogSourceFileResponse>(v)), Problem);
    }

    [HttpGet("download/{fileId}")]
    public async Task<ActionResult> DownloadFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileStream = await sender.Send(new GetLogSourceFileQuery(fileId), cancellationToken);

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
    [Authorize(Roles = "logsource_manager")]
    public async Task<ActionResult> DeleteFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileDeleted = await sender.Send(new DeleteLogSourceFileCommand(fileId), cancellationToken);

        return fileDeleted.Match(v => Ok(), Problem);
    }
}
