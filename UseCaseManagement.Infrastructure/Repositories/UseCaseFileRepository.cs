using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Repositories;

public class UseCaseFileRepository(UseCaseDbContext dbContext, IConfiguration configuration) : IUseCaseFileRepository
{
    public async Task<ErrorOr<Created>> AddUseCaseFile(UseCaseFile file, CancellationToken cancellationToken)
    {
        await dbContext.UseCaseFile.AddAsync(file, cancellationToken);
        return new Created();
    }

    public async Task DeleteUseCaseFile(Guid fileId, CancellationToken cancellationToken)
    {
        await dbContext.UseCaseFile.Where(x => x.Id == fileId).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<UseCaseFile?> GetUseCaseFileById(Guid fileId, CancellationToken cancellationToken)
    {
        return await dbContext.UseCaseFile.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
    }

    public async Task<FileStream> ReadFile(string uri, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await Task.FromResult(File.OpenRead(uri));
    }

    public async Task<string> SaveFile(Guid fileId, Guid useCaseId, FileRepresentation file, CancellationToken cancellationToken)
    {
        string basePath = configuration["Storage:UseCaseDirectory"]!;

        DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(basePath, useCaseId.ToString()));


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
