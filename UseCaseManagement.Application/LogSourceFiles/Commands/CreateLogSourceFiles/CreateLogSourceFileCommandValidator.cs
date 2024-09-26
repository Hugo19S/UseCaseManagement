using FluentValidation;

namespace UseCaseManagement.Application.LogSourceFiles.Commands.CreateLogSourceFiles;

public class CreateLogSourceFileCommandValidator : AbstractValidator<CreateLogSourceFileCommand>
{
    public CreateLogSourceFileCommandValidator()
    {
        RuleFor(x => x.LogSourceId)
        .NotEmpty().WithMessage("Enter the UseCaseId to associate the file.");

        RuleFor(x => x.File)
            .NotEmpty().WithMessage("The File can't be empty.");
    }
}
