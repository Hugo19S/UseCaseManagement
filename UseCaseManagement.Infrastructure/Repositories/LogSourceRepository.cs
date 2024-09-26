using ErrorOr;
using Microsoft.EntityFrameworkCore;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Repositories;

public class LogSourceRepository(UseCaseDbContext dbContext) : ILogSourceRepository
{
    public async Task<ErrorOr<Created>> AddLogSource(LogSource logSource, CancellationToken cancellationToken)
    {
        var logSourceSameName = await dbContext.LogSource.FirstOrDefaultAsync(x => x.Name == logSource.Name, cancellationToken);

        if (logSourceSameName != null)
        {
            return Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Name.");
        }

        await dbContext.LogSource.AddAsync(logSource, cancellationToken);

        return new Created();
    }

    public async Task DeleteLogSource(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.LogSource.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<LogSource>> GetAllLogSource(CancellationToken cancellationToken)
    {
        return await dbContext.LogSource.ToListAsync(cancellationToken);
    }

    public async Task<LogSource?> GetLogSourceById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.LogSource
            .Include(x => x.Files)
            .Include(x => x.UseCases)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ErrorOr<LogSource>> UpdateLogSource(LogSource logSource, List<Guid> useCasesIds, CancellationToken cancellationToken)
    {
        var sameTitle = await dbContext.LogSource.FirstOrDefaultAsync(x => x.Name == logSource.Name && x.Id != logSource.Id, cancellationToken);

        if (sameTitle != null)
        {
            return Error.Conflict("LogSource.Conflict", "There is already a LogSource with the same Title.");
        }

        try
        {
            List<UseCase> caseList = [];
            foreach (var id in useCasesIds)
            {
                var useCase = await dbContext.UseCase.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (useCase == null)
                {
                    return Error.NotFound("Usecase.NotFound", $"Usecase with id {id} not found.");
                }

                caseList.Add(useCase);
            }

            logSource.UseCases = caseList.Distinct().ToList();
        }
        catch (NullReferenceException)
        {
            logSource.UseCases = [];
        }

        return logSource;
    }
}
