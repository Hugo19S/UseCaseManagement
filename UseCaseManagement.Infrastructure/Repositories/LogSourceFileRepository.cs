using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Repositories;

public class LogSourceFileRepository(UseCaseDbContext dbContext, IConfiguration configuration) : ILogSourceFileRepository
{
    public async Task<ErrorOr<Created>> AddLogSourceFile(LogSourceFile file, CancellationToken cancellationToken)
    {
        await dbContext.LogSourceFile.AddAsync(file, cancellationToken);
        return new Created();
    }

    public async Task DeleteLogSourceFile(Guid fileId, CancellationToken cancellationToken)
    {
        await dbContext.LogSourceFile.Where(x => x.Id == fileId).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<LogSourceFile?> GetLogSourceFileById(Guid fileId, CancellationToken cancellationToken)
    {
        return await dbContext.LogSourceFile.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
    }

    public async Task<FileStream> ReadFile(string uri, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await Task.FromResult(File.OpenRead(uri));
    }

    public async Task<string> SaveFile(Guid fileId, Guid logSourceId, FileRepresentation file, CancellationToken cancellationToken)
    {
        string basePath = configuration["Storage:LogSourceDirectory"]!;

        DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(basePath, logSourceId.ToString()));

        
        string fileExtension = Path.GetExtension(file.Name);
        string filePath = Path.Combine(dir.FullName, fileId.ToString() + fileExtension);

        FileStream fs = File.Create(filePath);

        await file.Content.CopyToAsync(fs, cancellationToken);

        fs.Close();
        return filePath;
    }

    public void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }
}
