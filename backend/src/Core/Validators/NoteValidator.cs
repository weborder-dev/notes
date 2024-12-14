using FluentValidation;
using NotesBackend.Core.Models;

namespace NotesBackend.Core.Validators;

public class NoteValidator : AbstractValidator<Note>
{
    #region Constructors

    public NoteValidator()
    {
        RuleFor(n => n.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(n => n.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(n => n.Body)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(1024);
    }

    #endregion
}
