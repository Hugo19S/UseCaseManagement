using ErrorOr;
using MediatR;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Application.IRepositories;

namespace UseCaseManagement.Application.UseCases.Commands.DeleteUseCases;

public record DeleteUseCaseCommand(Guid Id) : IRequest<ErrorOr<Guid>>;

public class DeleteUseCaseCommandHandler(IUseCaseRepository useCaseRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUseCaseCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteUseCaseCommand request, CancellationToken cancellationToken)
    {
        var useCaseExist = await useCaseRepository.GetUseCaseById(request.Id, cancellationToken);

        if (useCaseExist == null)
        {
            return Error.NotFound("UseCase.NotFound", $"UseCase with id {request.Id} not found.");
        }

        await useCaseRepository.DeleteUseCase(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return request.Id;
    }
}
