using FluentValidation;

namespace UseCaseManagement.Application.UseCases.Queries.GetUseCase;

public class GetUseCaseQueryValidator : AbstractValidator<GetUseCaseQuery>
{
    public GetUseCaseQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The Id can't be empty.");
    }
}
