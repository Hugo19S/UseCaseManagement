using FluentValidation;

namespace UseCaseManagement.Application.UseCaseFiles.Commands.CreateUseCaseFiles;

public class CretateUseCaseFileCommandValidator : AbstractValidator<CreateUseCaseFileCommand>
{
    public CretateUseCaseFileCommandValidator()
    {
        RuleFor(x => x.UseCaseId)
            .NotEmpty().WithMessage("Enter the UseCaseId to associate the file.");

        RuleFor(x => x.File)
            .NotEmpty().WithMessage("The File can't be empty.");
    }
}
