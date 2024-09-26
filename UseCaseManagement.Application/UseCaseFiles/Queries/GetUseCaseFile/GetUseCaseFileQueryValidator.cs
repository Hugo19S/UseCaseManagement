using FluentValidation;

namespace UseCaseManagement.Application.UseCaseFiles.Queries.GetUseCaseFile;

public class GetUseCaseFileQueryValidator : AbstractValidator<GetUseCaseFileQuery>
{
    public GetUseCaseFileQueryValidator()
    {
        RuleFor(x => x.FileId)
            .NotEmpty().WithMessage("Enter the FileId to get file");
    }
}
