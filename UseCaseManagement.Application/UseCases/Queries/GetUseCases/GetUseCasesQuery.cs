using ErrorOr;
using MediatR;
using UseCaseManagement.Application.IRepositories;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Domain.Filters;

namespace UseCaseManagement.Application.UseCases.Queries.GetUseCases;

public record GetUseCasesQuery(UseCaseFilter UseCaseFilter) : IRequest<ErrorOr<List<UseCase>>>;
public class GetUseCasesQueryHandler(IUseCaseRepository useCaseRepository) : IRequestHandler<GetUseCasesQuery, ErrorOr<List<UseCase>>>
{
    public async Task<ErrorOr<List<UseCase>>> Handle(GetUseCasesQuery request, CancellationToken cancellationToken)
    {
        return await useCaseRepository.GetAllUseCase(request.UseCaseFilter, cancellationToken);
    }
}
