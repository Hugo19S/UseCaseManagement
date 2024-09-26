using FluentValidation;

namespace UseCaseManagement.Application.LogSourceFiles.Commands.DeleteLogSourceFiles;

public class DeleteLogSourceFileCommandValidator : AbstractValidator<DeleteLogSourceFileCommand>
{
    public DeleteLogSourceFileCommandValidator()
    {
        RuleFor(x => x.FileId)
            .NotEmpty().WithMessage("Enter the FileId to be deleted");
    }
}
