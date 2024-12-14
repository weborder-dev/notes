using NotesBackend.Core.Models;

namespace NotesBackend.Core.Abstractions;

public interface INotesService
{
    #region Public Methods

    Task<ServiceResult<Note>> CreateNoteAsync(Note note);
    Task<ServiceResult<Note>> UpdateNoteAsync(Note note);
    Task<ServiceResult<Note>> GetNoteByIdAsync(string id);
    Task<ServiceResult<IList<Note>>> GetAllNotesAsync();
    Task DeleteNoteAsync(string id);
        
    #endregion
}