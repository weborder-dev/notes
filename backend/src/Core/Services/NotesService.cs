using FluentValidation;
using NotesBackend.Core.Abstractions;
using NotesBackend.Core.Models;

namespace NotesBackend.Core.Services;

public class NotesService : INotesService
{
    #region Fields

    private readonly INotesStore _store;
    private readonly IValidator<Note> _validator;
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public NotesService(
        INotesStore store,
        IValidator<Note> validator,
        ILogger<NotesService> logger)
    {
        _store = store;
        _validator = validator;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task<ServiceResult<Note>> CreateNoteAsync(Note note)
    {
        var valResult = await _validator.ValidateAsync(note);
        if (!valResult.IsValid)
        {
            var errs = valResult.Errors
                .Select(err => err.ErrorMessage)
                .ToList();

            return ServiceResult<Note>.Failure(errs);    
        }

        var insertedNote = await _store.CreateNoteAsync(note);

        return ServiceResult<Note>.Success(insertedNote);
    }

    public async Task DeleteNoteAsync(string id)
    {
        await _store.DeleteNote(id);
    }

    public async Task<ServiceResult<IList<Note>>> GetAllNotesAsync()
    {
        var notes = await _store.GetAllNotesAsync();

        return ServiceResult<IList<Note>>.Success(notes.ToList());
    }

    public async Task<ServiceResult<Note>> GetNoteByIdAsync(string id)
    {
        var note = await _store.GetNoteByIdAsync(id);
        
        return note is not null
            ? ServiceResult<Note>.Success(note)
            : ServiceResult<Note>.Failure("not found");
    }

    public Task<ServiceResult<Note>> UpdateNoteAsync(Note note)
    {
        throw new NotImplementedException();
    }

    #endregion
}