using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Application.UseCases.Queries.GetUseCase;

public record GetUseCaseQuery(Guid Id) : IRequest<ErrorOr<UseCase>>;

public class GetUseCaseQueryHandler(IUseCaseRepository useCaseRepository) : IRequestHandler<GetUseCaseQuery, ErrorOr<UseCase>>
{
    public async Task<ErrorOr<UseCase>> Handle(GetUseCaseQuery request, CancellationToken cancellationToken)
    {
        var useCase = await useCaseRepository.GetUseCaseById(request.Id, cancellationToken);

        if (useCase == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.Id} not found.");
        }

        return useCase;
    }
}
