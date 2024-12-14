using NotesBackend.Core.Models;

namespace NotesBackend.Core.Abstractions;

public interface INotesStore
{
    Task<Note> CreateNoteAsync(Note note);
    Task<Note?> GetNoteByIdAsync(string id);
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Note?> UpdateNoteAsync(Note note);
    Task<bool> DeleteNote(string id);
}