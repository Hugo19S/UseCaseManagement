using FluentValidation;

namespace UseCaseManagement.Application.UseCaseFiles.Commands.DeleteUseCaseFiles;

public class DeleteUseCaseFileCommandValidator : AbstractValidator<DeleteUseCaseFileCommand>
{
    public DeleteUseCaseFileCommandValidator()
    {
        RuleFor(x => x.FileId)
            .NotEmpty().WithMessage("Enter the FileId to be deleted");
    }
}
