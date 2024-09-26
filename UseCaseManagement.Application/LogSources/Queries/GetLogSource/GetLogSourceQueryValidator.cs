using FluentValidation;

namespace UseCaseManagement.Application.LogSources.Queries.GetLogSource;

public class GetLogSourceQueryValidator : AbstractValidator<GetLogSourceQuery>
{
    public GetLogSourceQueryValidator()
    {
        RuleFor(x => x.LogSourceId)
            .NotEmpty().WithMessage("The LogSourceId can't be empty."); ;
    }
}
