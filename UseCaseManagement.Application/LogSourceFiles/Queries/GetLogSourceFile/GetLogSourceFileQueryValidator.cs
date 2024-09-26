using FluentValidation;

namespace UseCaseManagement.Application.LogSourceFiles.Queries.GetLogSourceFile;

public class GetLogSourceFileQueryValidator : AbstractValidator<GetLogSourceFileQuery>
{
    public GetLogSourceFileQueryValidator()
    {
        RuleFor(x => x.FileId)
            .NotEmpty().WithMessage("Enter the FileId to get file");
    }
}
