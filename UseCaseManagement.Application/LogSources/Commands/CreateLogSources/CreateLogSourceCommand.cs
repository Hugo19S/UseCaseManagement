using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.LogSources.Commands.CreateLogSources;

public record CreateLogSourceCommand(string Name, string Description) : IRequest<ErrorOr<LogSource>>;

public class CreateLogSourceCommandHandler(ILogSourceRepository sourceRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateLogSourceCommand, ErrorOr<LogSource>>
{
    public async Task<ErrorOr<LogSource>> Handle(CreateLogSourceCommand request, CancellationToken cancellationToken)
    {
        var createLogSource = new LogSource
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        var logSourcecreated = await sourceRepository.AddLogSource(createLogSource, cancellationToken);

        if (logSourcecreated.IsError)
        {
            return logSourcecreated.Errors;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return createLogSource;
    }
}
